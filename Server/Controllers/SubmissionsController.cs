using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoryForce.Server.Services;
using StoryForce.Server.ViewModels;
using StoryForce.Shared.Dtos;
using StoryForce.Shared.Models;
using StoryForce.Shared.Services;
using StoryForce.Shared.ViewModels;
using File = Google.Apis.Drive.v3.Data.File;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly SubmissionService _submissionService;
        private readonly StoryFileService _storyFileService;
        private readonly PeopleService _peopleService;
        private readonly EventService _eventService;
        private readonly IConfiguration _configuration;
        readonly string[] scopes = { DriveService.Scope.Drive };
        DriveService _gDriveService;
        private FileService _fileService;
        private WebClient _webClient;
        private string UPLOAD_DIRECTORY;

        public SubmissionsController(IConfiguration configration
            , SubmissionService submissionService
            , StoryFileService storyFileService
            , PeopleService peopleService
            , EventService eventService)
        {
            _configuration = configration;
            _submissionService = submissionService;
            _storyFileService = storyFileService;
            _peopleService = peopleService;
            _gDriveService = InitGoogleDriveService();
            _eventService = eventService;
            _fileService = new FileService();
            _webClient = new WebClient();
            this.UPLOAD_DIRECTORY = Path.Combine(Path.GetTempPath(), "uploads");
        }

        [HttpGet]
        public async Task<ActionResult<List<SubmissionDto>>> Get()
        {
            var submissions = await _submissionService.GetAsync();

            return submissions.Select(submission => SubmissionDto.ConvertFromEntity(submission)).ToList();
        }

        [HttpGet("{id:length(24)}", Name = "GetSubmission")]
        public async Task<ActionResult<SubmissionDto>> Get(string id)
        {
            var submission = await _submissionService.GetAsync(id);

            if (submission == null)
            {
                return NotFound();
            }

            return SubmissionDto.ConvertFromEntity(submission);
        }

        [HttpPost("simple")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)] //2GB:1024 * 1024 * 1024 * 2
        public async Task<ActionResult<SubmissionDto>> CreateFromSimple(BlazorFilesSubmission submission)
        {
            var events = await _eventService.GetAsync();
            var people = await _peopleService.GetAsync();
            var converted = submission.ConvertToEntity();
            var submitter = await _peopleService.CreateAsync(converted.SubmittedBy);
            converted.SubmittedBy = submitter;

            foreach (var file in submission.UploadFiles)
            {
                var storyFile = converted.SubmittedFiles.SingleOrDefault(f => f.Title == file.Title);

                storyFile.UpdatedAt = converted.CreatedAt;
                storyFile.SubmissionId = converted.Id;
                storyFile.SubmittedBy = submitter;
                var fEvent = events.SingleOrDefault(e => e.Name == file.EventName);
                var fRequestedBy = people.SingleOrDefault(e => e.Name == file.RequestedBy);
                storyFile.Event = fEvent;
                storyFile.RequestedBy = fRequestedBy;
                storyFile.Class = file.Class;

                foreach (var person in storyFile.FeaturedPeople)
                {
                    var featured = people.Find(p => p.Name == person.Name);
                    if (featured is null)
                    {
                        continue;
                    }
                    person.Id = featured.Id;
                    person.Email = featured.Email;
                    person.Type = featured.Type;
                    person.ClassOfYear = featured.ClassOfYear;
                    person.AvatarUrl = featured.AvatarUrl;
                }
            }

            await _storyFileService.CreateMultipleAsync(converted.SubmittedFiles);

            await _submissionService.CreateAsync(converted);

            return CreatedAtRoute("GetSubmission", new { id = converted.Id }, converted);
        }

        [HttpPost("blazor")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)] //2GB:1024 * 1024 * 1024 * 2
        public async Task<ActionResult<SubmissionDto>> CreateFromBlazor(BlazorFilesSubmission submission)
        {
            var googleDriveId = this._configuration.GetSection("Google:Drive:DriveId").Value;
            var converted = submission.ConvertToEntity();
            var submitter = await _peopleService.CreateAsync(converted.SubmittedBy);
            converted.SubmittedBy = submitter;

            for (var index = 0; index < submission.UploadFiles.Count; index++)
            {
                var currentFile = submission.UploadFiles[index];
                if (currentFile.StorageProvider == StorageProvider.LocalFileSystem && currentFile.Size == 0)
                {
                    continue;
                }

                File newFile = new File
                {
                    Name = currentFile.Title,
                    Description = converted.SubmittedFiles != null && converted.SubmittedFiles.Count > 0
                        ? converted.SubmittedFiles[index].Description
                        : string.Empty,
                    Parents = new List<string>() { googleDriveId }
                };

                Stream readStream = null;

                if (currentFile.StorageProvider == StorageProvider.LocalFileSystem)
                {
                    newFile.MimeType = currentFile.MimeType;
                    readStream = new MemoryStream();
                }
                else
                {
                    newFile.MimeType = MimeTypesMap.GetMimeType(newFile.Name);
                    string downloadUrl = string.Empty;

                    if (currentFile.StorageProvider == StorageProvider.GoogleDrive)
                    {
                        downloadUrl =
                            $"https://lh3.googleusercontent.com/d/{currentFile.ProviderFileId}" +
                            (submission.GDriveOAuthToken != null
                                ? "?access_token=" + submission.GDriveOAuthToken
                                : string.Empty);
                    }

                    if (currentFile.StorageProvider == StorageProvider.Url)
                    {
                        downloadUrl = currentFile.DownloadUrl;
                    }

                    readStream = await _webClient.OpenReadTaskAsync(new Uri(downloadUrl));
                }

                var tempFilename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
                await using (var stream = new FileStream(tempFilename, FileMode.CreateNew))
                {
                    const int chunkSize = 1024;
                    var buffer = new byte[chunkSize];
                    var bytesRead = 0;
                    do
                    {
                        bytesRead = await readStream.ReadAsync(buffer, 0, buffer.Length);
                        await stream.WriteAsync(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    await readStream.DisposeAsync();
                }

                var uploadStream = new FileStream(tempFilename, FileMode.Open);

                var request = _gDriveService.Files.Create(newFile, uploadStream, currentFile.MimeType);
                request.ChunkSize = 1 * 1024 * 1024; //1MB per chunk
                request.Fields = "id, name, webViewLink, webContentLink, thumbnailLink, createdTime, size";

                request.ResponseReceived += (file) =>
                {
                    var storyFile = converted.SubmittedFiles.SingleOrDefault(f => f.Title == file.Name);
                    storyFile.DownloadUrl = file.WebContentLink;
                    storyFile.ThumbnailUrl = file.ThumbnailLink;
                    storyFile.UpdatedAt = converted.CreatedAt;
                    storyFile.SubmissionId = converted.Id;
                    storyFile.SubmittedBy = submitter;
                    storyFile.Size = file.Size;
                };

                await request.UploadAsync();
            }

            // await _peopleService.CreateMultipleAsync(converted.FeaturedPeople);
            foreach (var person in converted.FeaturedPeople)
            {
                var addedPerson = await _peopleService.CreateAsync(person);
                person.Id = addedPerson.Id;
            }

            await _storyFileService.CreateMultipleAsync(converted.SubmittedFiles);

            if (converted.Event != null && !string.IsNullOrEmpty(converted.Event.Name))
            {
                await _eventService.CreateAsync(converted.Event);
            }

            await _submissionService.CreateAsync(converted);

            return CreatedAtRoute("GetSubmission", new { id = converted.Id }, converted);
        }

        private async Task SaveFileChunkToDisk(IFormFile file)
        {
            var tempFilename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
            await using var stream = new FileStream(tempFilename, FileMode.CreateNew);
            await file.CopyToAsync(stream);
        }

        private async Task UploadFileToGoogleDrive(Submission converted, Person submitter, UploadFile currentFile, string readFilePath)
        {
            var googleDriveId = this._configuration.GetSection("Google:Drive:DriveId").Value;

            File newFile = new File
            {
                Name = currentFile.Title,
                Description = currentFile.Description,
                Parents = new List<string>() { googleDriveId }
            };
            var uploadStream = new FileStream(readFilePath, FileMode.Open);

            var request = _gDriveService.Files.Create(newFile, uploadStream, currentFile.MimeType);
            request.ChunkSize = 1 * 1024 * 1024; //1MB per chunk
            request.Fields = "id, name, webViewLink, webContentLink, thumbnailLink, createdTime, size";

            request.ResponseReceived += (file) =>
            {
                var storyFile = converted.SubmittedFiles.SingleOrDefault(f => f.Title == file.Name);
                storyFile.DownloadUrl = file.WebContentLink;
                storyFile.ThumbnailUrl = file.ThumbnailLink;
                storyFile.UpdatedAt = converted.CreatedAt;
                storyFile.SubmissionId = converted.Id;
                storyFile.SubmittedBy = submitter;
                storyFile.Size = file.Size;
            };

            await request.UploadAsync();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)] //2GB:1024 * 1024 * 1024 * 2
        public async Task<ActionResult<SubmissionDto>> Create([FromForm] FilesSubmission submission)
        {
            var googleDriveId = this._configuration.GetSection("Google:Drive:DriveId").Value;
            var converted = submission.ConvertToEntity();
            var submitter = await _peopleService.CreateAsync(converted.SubmittedBy);
            converted.SubmittedBy = submitter;

            for (var index = 0; index < submission.FormFiles.Count; index++)
            {
                var currentFile = submission.FormFiles[index];
                File newFile = new File
                {
                    Name = currentFile.FileName,
                    Description = converted.SubmittedFiles != null && converted.SubmittedFiles.Count > 0
                        ? converted.SubmittedFiles[index].Description
                        : string.Empty,
                    Parents = new List<string>() { googleDriveId }
                };

                Stream readStream;

                if (currentFile.Length > 0)
                {
                    newFile.MimeType = currentFile.ContentType;
                    readStream = currentFile.OpenReadStream();
                }
                else
                {
                    newFile.MimeType = MimeTypesMap.GetMimeType(newFile.Name);
                    var fileMeta = submission.FileMetaDataList[index];
                    string downloadUrl = string.Empty;
                    if (fileMeta.StorageProvider == StorageProvider.GoogleDrive)
                    {
                        downloadUrl =
                            $"https://lh3.googleusercontent.com/d/{fileMeta.FileId}" +
                            (submission.GDriveOAuthToken != null
                                ? "?access_token=" + submission.GDriveOAuthToken
                                : string.Empty);
                    }
                    else if (fileMeta.StorageProvider == StorageProvider.Url)
                    {
                        downloadUrl = fileMeta.DownloadUrl;
                    }

                    readStream = await _webClient.OpenReadTaskAsync(new Uri(downloadUrl));
                }

                var tempFilename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
                await using (var stream = new FileStream(tempFilename, FileMode.CreateNew))
                {
                    const int chunkSize = 1024;
                    var buffer = new byte[chunkSize];
                    var bytesRead = 0;
                    do
                    {
                        bytesRead = await readStream.ReadAsync(buffer, 0, buffer.Length);
                        await stream.WriteAsync(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    await readStream.DisposeAsync();
                }

                var uploadStream = new FileStream(tempFilename, FileMode.Open);

                var request = _gDriveService.Files.Create(newFile, uploadStream, currentFile.ContentType);
                request.ChunkSize = 1 * 1024 * 1024; //1MB per chunk
                request.Fields = "id, name, webViewLink, webContentLink, thumbnailLink, createdTime, size";

                request.ResponseReceived += (file) =>
                {
                    var storyFile = converted.SubmittedFiles.SingleOrDefault(f => f.Title == file.Name);
                    storyFile.DownloadUrl = file.WebContentLink;
                    storyFile.ThumbnailUrl = file.ThumbnailLink;
                    storyFile.UpdatedAt = converted.CreatedAt;
                    storyFile.SubmissionId = converted.Id;
                    storyFile.SubmittedBy = submitter;
                    storyFile.Size = file.Size;
                };

                await request.UploadAsync();
            }

            // await _peopleService.CreateMultipleAsync(converted.FeaturedPeople);
            foreach (var person in converted.FeaturedPeople)
            {
                var addedPerson = await _peopleService.CreateAsync(person);
                person.Id = addedPerson.Id;
            }

            await _storyFileService.CreateMultipleAsync(converted.SubmittedFiles);

            if (converted.Event != null && !string.IsNullOrEmpty(converted.Event.Name))
            {
                await _eventService.CreateAsync(converted.Event);
            }

            await _submissionService.CreateAsync(converted);

            return CreatedAtRoute("GetSubmission", new { id = converted.Id }, converted);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, SubmissionDto updatedSubmission)
        {
            var submission = await _submissionService.GetAsync(id);

            if (submission == null)
            {
                return NotFound();
            }

            await _submissionService.UpdateAsync(id, updatedSubmission.ConvertToEntity());

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var submission = await _submissionService.GetAsync(id);

            if (submission == null)
            {
                return NotFound();
            }

            await _submissionService.RemoveAsync(id);

            return NoContent();
        }


        private DriveService InitGoogleDriveService()
        {
            var serviceAccount = this._configuration.GetSection("Google:ServiceAccount:Email").Value;
            var privateKey = this._configuration.GetSection("Google:ServiceAccount:Key").Value;
            ServiceAccountCredential credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccount)
                {
                    Scopes = scopes
                }.FromPrivateKey(privateKey));

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Brandeis Story Force",
            });

            return service;
        }        

    }
}
