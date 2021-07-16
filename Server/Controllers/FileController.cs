using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using HeyRed.Mime;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using StoryForce.Server.Services;
using StoryForce.Server.ViewModels;
using StoryForce.Shared;
using StoryForce.Shared.Models;
using StoryForce.Shared.ViewModels;
using File = Google.Apis.Drive.v3.Data.File;

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        private IConfiguration _configuration;
        private DriveService _gDriveService;
        private IHostingEnvironment hostingEnvironment;
        private string _googleDriveId;
        private WebClient _webClient;
        private string UPLOAD_DIRECTORY;
        private readonly IStoryFileService _storyFileService;       
        public FileController(IHostingEnvironment hostingEnvironment, IConfiguration configration, IStoryFileService storyFileService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this._configuration = configration;
            this._gDriveService = InitGoogleDriveService();
            this._googleDriveId = this._configuration.GetSection("Google:Drive:DriveId").Value;
            this._webClient = new WebClient();
            this.UPLOAD_DIRECTORY = Path.Combine(Path.GetTempPath(), "uploads");
            this._storyFileService = storyFileService;            
        }

        [HttpGet("UploadByUrl")]
        public async Task<IActionResult> UploadFileByUrl(string url, string fileName, string description)
        {
            string filename = url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
            // check for allowable file extensions
            if (!Constants.UploadableFiles.Split(',')
                .Contains(Path.GetExtension(filename).ToLower().Replace(".", "")))
            {
                return Json(new { message = $"File Could Not Be Downloaded From Url Due To Its File Extension {Url}" });
            }

            if (!filename.IsPathOrFileValid())
            {
                return Json(new { message = $"File Could Not Be Downloaded From Url Due To Its File Name Not Allowed {url}" });
            }

            File newFile = new File
            {
                Name = fileName,
                Description =description,
                Parents = new List<string>() { _googleDriveId }
            };

            try
            {
                newFile.MimeType = MimeTypesMap.GetMimeType(url);

                string targetPath = Path.Combine(Path.GetTempPath(), $"{filename}{Guid.NewGuid()}.tmp");
                // remove file if it already exists
                if (System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Delete(targetPath);
                }

                _webClient.DownloadFile(url, targetPath);

                var uploadStream = new FileStream(targetPath, FileMode.Open);

                var request = _gDriveService.Files.Create(newFile, uploadStream, newFile.MimeType);
                request.ChunkSize = 1 * 1024 * 1024; //1MB per chunk
                request.Fields = "id, name, webViewLink, webContentLink, thumbnailLink, createdTime, size";

                var uploadFile = new UploadFile();
                request.ResponseReceived += (file) =>
                {
                    uploadFile.Title = file.Name;
                    uploadFile.Description = file.Description;
                    uploadFile.DownloadUrl = file.WebContentLink;
                    uploadFile.ThumbnailUrl = file.ThumbnailLink;
                    uploadFile.Size = file.Size;
                };

                await request.UploadAsync();
                return Json(new { message = $"File Uploaded Successfully.", stats = "upload_completed", uploadFile });
            }
            catch
            {
                return Json(new { message = $"File Could Not Be Downloaded From Url {Url}" });
            }
        }

        [HttpPost("UploadFileChunk")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)] //2GB:1024 * 1024 * 1024 * 2
        public async Task<UploadStatus> UploadFileChunk(IFormFile file, string description, long totalSize)
        {
            if (file.Length == 0)
            {
                return new UploadStatus {Message = "The file is empty", Status = "error"};
            }

            var uploadedFileName = SaveFile(file);
            return new UploadStatus
                { Message = "File Chunk Uploaded Successfully!", Status = "success" };
        }

        [HttpPost("UploadByUrls")]
        public async Task<string[]> UploadByUrls(UploadByUrl[] files)
        {
            var uploads = new List<string>();
            var utcTicks = DateTime.UtcNow.Ticks;
            foreach (var file in files)
            {
                await using var readStream = await _webClient.OpenReadTaskAsync(new Uri(file.DownloadUrl));
                var tempFilename = Path.Combine(UPLOAD_DIRECTORY, $"{utcTicks}-{file.Title}");
                uploads.Add(tempFilename);
                await using (var writeStream = new FileStream(tempFilename, FileMode.CreateNew))
                {
                    const int chunkSize = 1024;
                    var buffer = new byte[chunkSize];
                    var bytesRead = 0;
                    do
                    {
                        bytesRead = await readStream.ReadAsync(buffer, 0, buffer.Length);
                        await writeStream.WriteAsync(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    await readStream.DisposeAsync();
                    await writeStream.DisposeAsync();
                }
            }
            return uploads.ToArray();
        }

        public async Task<UploadStatus> UploadFileToGoogleDrive(string fileOnDisk, string fileName, string description, string mimeType) 
        {
            File newFile = new File
            {
                Name = fileName.GetFileNameWithoutTimeStamp(),
                Description = description,
                MimeType = mimeType,
                Parents = new List<string>() { _googleDriveId }
            };

            await using var uploadStream = new FileStream(fileOnDisk, FileMode.Open, FileAccess.Read);

            var request = _gDriveService.Files.Create(newFile, uploadStream, mimeType);
            request.ChunkSize = 10 * 1024 * 1024; //10MB per chunk
            request.Fields = "id, name, webViewLink, webContentLink, thumbnailLink, createdTime, size";

            var uploadFile = new UploadFile();
            request.ResponseReceived += (file) =>
            {
                uploadFile.Title = file.Name;
                uploadFile.ProviderFileId = file.Id;
                uploadFile.Description = file.Description;
                uploadFile.DownloadUrl = file.WebContentLink;
                uploadFile.ThumbnailUrl = file.ThumbnailLink;
                uploadFile.Size = file.Size;
            };

            await request.UploadAsync();

            //System.IO.File.Delete(uploadedFileName);

            return new UploadStatus { Message = "File Uploaded to Google Drive", Status = "success", };
        }

        private async Task DeleteFiles(string dir, string fileName)
        {
            const string token = ".part_";
            string[] fileParts = Directory.GetFiles(dir, fileName + token + "*"); // list of all file parts
            foreach (var file in fileParts)
            {
                System.IO.File.Delete(file);
            }
        }

        private string SaveFile(IFormFile uploadedFile)
        {
            string uploadDirectory = Path.Combine(Path.GetTempPath(), "uploads");

            var fileName = Path.Combine(uploadDirectory, uploadedFile.FileName);

            if (!System.IO.File.Exists(fileName))
            {
                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    uploadedFile.CopyTo(fs);
                    fs.Flush();
                }
            }
            else
            {
                using (FileStream fs = System.IO.File.Open(fileName, FileMode.Append))
                {
                    uploadedFile.CopyTo(fs);
                    fs.Flush();
                }
            }

            // clean up file parts which are more than 2 hours old ( which can happen if a prior file upload failed )
            var cleanupFiles = Directory.EnumerateFiles(uploadDirectory, "*");

            foreach (var file in cleanupFiles)
            {
                var createdDate = System.IO.File.GetCreationTime(file).ToUniversalTime();
                if (createdDate < DateTime.UtcNow.AddHours(-2))
                {
                    System.IO.File.Delete(file);
                }
            }

            return fileName;
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            filename = filename.Replace(" ", string.Empty);
            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string path = this.hostingEnvironment.WebRootPath + "\\upload\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }

        private DriveService InitGoogleDriveService()
        {
            var serviceAccount = this._configuration.GetSection("Google:ServiceAccount:Email").Value;
            var privateKey = this._configuration.GetSection("Google:ServiceAccount:Key").Value;
            ServiceAccountCredential credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccount)
                {
                    Scopes = new []{ DriveService.Scope.Drive }
                }.FromPrivateKey(privateKey));

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Brandeis Story Force",
            });

            return service;
        }
        [HttpPost("{id}/favorite")]
        public async Task<ActionResult> Favourite(int id)
        {
            var storyFile = await this._storyFileService.GetAsync(id);     
            if(storyFile.FavouritesPeople.Count > 0)
            {
                storyFile.FavouritesPeople.Remove((Person)storyFile.FavouritesPeople);
                await _storyFileService.UpdateAsync(id, storyFile);
                return Ok();
            }
            else
            {
               // storyFile.FavouritesPeople.Add(storyFile);
            }
            return Ok();
        }

    }
}
