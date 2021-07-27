using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HeyRed.Mime;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Bson.IO;
using Sentry;
using StoryForce.Shared.Dtos;
using StoryForce.Shared.ViewModels;

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : Controller
    {
        private readonly IConfiguration _configuration;
        private IAmazonS3 _s3Client;
        private string _s3BucketName;

        public S3Controller(IConfiguration configuration, IAmazonS3 s3Client)
        {
            this._configuration = configuration;
            this._s3BucketName = this._configuration.GetSection("AWS:S3:BucketName").Value;
            this._s3Client = s3Client;
        }
        public IActionResult Index()
        {
            return new JsonResult("Nothing here yet.");
        }

        [HttpGet("GetPreSignedBucketUrl")]
        public IActionResult GetPreSignedBucketUrl()
        {
            var url = this._s3Client.GetPreSignedURL(
                new GetPreSignedUrlRequest
                {
                    BucketName = this._s3BucketName,

                    Verb = HttpVerb.PUT,
                    Expires = DateTime.UtcNow.AddHours(2)
                });

            return new JsonResult(url);
        }

        [HttpGet("GetPreSignedUrl")]
        public IActionResult GetPreSignedUrl(string fileName, string mimeType)
        {
            var url = this._s3Client.GetPreSignedURL(
                new GetPreSignedUrlRequest
            {
                BucketName = this._s3BucketName,
                Key = fileName,
                ContentType = mimeType,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddHours(2)
            });

            return new JsonResult(url);
        }

        [HttpGet("GetPreSignedUrls")]
        public IActionResult GetPreSignedUrls([FromQuery]string[] fileNames)
        {
            var urls = new string[fileNames.Length];

            for (var index = 0; index < fileNames.Length; index++)
            {
                var url = this._s3Client.GetPreSignedURL(
                    new GetPreSignedUrlRequest
                    {
                        BucketName = this._s3BucketName,
                        Key = fileNames[index],
                        Verb = HttpVerb.PUT,
                        Expires = DateTime.UtcNow.AddHours(2)
                    });

                urls[index] = url;
            }

            return new JsonResult(urls);
        }

        [HttpPost("UploadByUrls")]
        public IActionResult UploadByUrls(UrlsWithAccessToken dto)
        {
            var uploads = new List<string>();
            const long chunkSize = 5 * 1024 * 1024; // 5 MB

            Parallel.ForEach(dto.UploadByUrls, async (file) => 
            {
                uploads.Add(file.Key);
                var webClient = new WebClient();
                if (!string.IsNullOrEmpty(file.AccessToken))
                {
                    webClient.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {file.AccessToken}");
                }

                if (file.Size.GetValueOrDefault() < chunkSize)
                {
                    webClient.DownloadDataAsync(new Uri(file.DownloadUrl));
                    webClient.DownloadDataCompleted += async delegate(object sender, DownloadDataCompletedEventArgs args)
                    {
                        await Upload(file.Key, args.Result);
                    };
                }
                else
                {
                    // Create list to store upload part responses.
                    List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

                    string targetPath = Path.Combine(Path.Combine(Path.GetTempPath(), "uploads"), $"{file.Key}");
                    await webClient.DownloadFileTaskAsync(new Uri(file.DownloadUrl), targetPath);

                    var initiateRequest = new InitiateMultipartUploadRequest
                    {
                        BucketName = _s3BucketName,
                        Key = file.Key
                    };

                    var initResponse =
                        await _s3Client.InitiateMultipartUploadAsync(initiateRequest);

                    try
                    {
                        long filePosition = 0;
                        for (var i = 1; filePosition < file.Size; i++)
                        {
                            var uploadRequest = new UploadPartRequest
                            {
                                BucketName = _s3BucketName,
                                Key = file.Key,
                                UploadId = initResponse.UploadId,
                                PartNumber = i,
                                PartSize = chunkSize,
                                FilePosition = filePosition,
                                FilePath = targetPath
                            };

                            //await writeStream.WriteAsync(buffer, 0, bytesRead);
                            var uploadPartResponse = await _s3Client.UploadPartAsync(uploadRequest);

                            uploadResponses.Add(uploadPartResponse);

                            filePosition += chunkSize;
                        }

                        // Setup to complete the upload.
                        CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest
                        {
                            BucketName = _s3BucketName,
                            Key = file.Key,
                            UploadId = initResponse.UploadId
                        };

                        completeRequest.AddPartETags(uploadResponses);

                        // Complete the upload.
                        CompleteMultipartUploadResponse completeUploadResponse =
                            await _s3Client.CompleteMultipartUploadAsync(completeRequest);

                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"An AmazonS3Exception was thrown: {exception.Message}");

                        // Abort the upload.
                        AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
                        {
                            BucketName = _s3BucketName,
                            Key = file.Key,
                            UploadId = initResponse.UploadId
                        };
                        await _s3Client.AbortMultipartUploadAsync(abortMPURequest);

                        SentrySdk.CaptureException(exception);
                    }
                }
            });

            return new JsonResult(uploads);
        }

        private async Task Upload(string fileKey, byte[] data)
        {
            var fileTransferUtility =
                new TransferUtility(_s3Client);

            try
            {
                await fileTransferUtility.UploadAsync(new MemoryStream(data), _s3BucketName, fileKey);
            }
            catch (Exception err)
            {
                Console.WriteLine($"An AmazonS3Exception was thrown: { err.Message}");
            }
        }
    }
}
