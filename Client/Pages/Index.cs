using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using StoryForce.Client.ViewModels;
using StoryForce.Shared.Dtos;
using StoryForce.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Tewr.Blazor.FileReader;
using File = Google.Apis.Drive.v3.Data.File;

namespace StoryForce.Client.Pages
{
    public partial class Index
    {
        string[] scopes = {DriveService.Scope.Drive};
        string serviceAccount = "story-force-service@brandeis-story-f-1595602597421.iam.gserviceaccount.com";
        string driveId = "1OZsjbPxXNM34W6Ii74NV4d_fsbkyFOUM";
        string privateKey = @"-----BEGIN PRIVATE KEY-----MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQC8wKbKRf9n6YTgvFacPQgEP1vBrFTIi11T29rblOXe0RMXgvfL2d5zcUrSs+zX3nPjilwU2UlWriiULKHh9CrwHPSBHmXG8+qthyapyhQffmabaCxBfzsU6U8PwdhicXKd+MQAP5pn37O/wEpXudp9dqcBRY6THMhsOqs0YFCte+FJHSZxsQpnZ1BKqVSYsL/Zw7u7Ek5plaI3GDUYOKV3tRDfqYE7QHRoO2UyUIapxhsz+qqHB3CSF3gifYGIB5c0sa7MmvQyVgVUQKPzRDkEDXw3SJyLF/PSYbdO/6Ff7p3NaswmGh2SIqgmtpCy5qSyTVdSXTFQxq+dG+GBF/zLAgMBAAECggEAJBWsjUT+g+4X0iMPZ8UfMDiwoQECIGjMScjhOVNo8sUDol4OY3FeXrGM/WUaZVaIzzpXSkEUDTc0WMeDlz/nqYnPkJOwH23oMo6A7LrOSAsRMKqks0zQjbngvIFxjbhkkiDJz8ZZDXytz33CeKz2QUAaw1p53FQHWPGKY3P6WH/e3VrrG7wWurAA4+yF5WN5WAesijOO6sJDkqHwc6ETwqnxSqnVz+a/CJzLZ933kxmjVcJhJIKJ91vl4WlZVT57gb37ZL9e8ebrcaObUl6UxcRC2iZ3ocJz5LZ5HPoV6ixZgaP0gZCI/DeRWq8NwBJxhg+DEtebCwH0fLyCFTsmQQKBgQDzwLwYWJVEAp4K90y0l1V+U+huuJEVAlWIFEfco1DAW0qvQsqFA0vNl5DHxiQjCSswQbbApLmcvwx2eSSeMejxs0LJxO0FG6Mex76qj16YJxOJxjWFhVn2XOP39m9OxM6bi7hr3tsqFY7jo8DvYagoH0g145oRuaz+8GmrOog16wKBgQDGPHn5LqPy4UmYnEHChIvujmWoodDTw94e+wqjVJyNUUKkDBVD0dzuQrRMs0VuNcd47KG4vvES+88NZ2gTHbrBsq+px+8+KEDRfer1KUHmKdfVF5LB2s/Ayotrn9lZ2q94/Zse7Jsm9h6TM9tInex1P8bj4egMJXRMcEbslKE8oQKBgQC3GDMx5nhY2c51VkWb8YEan1Sctq7kJRiyWumP5m0l0G1NMNKHD27FQ/BQ35kNaDm0yefI1PNHtopaA61p1vuvcoPco7uWdqgU2t1xqLfZeqDGgMh7PyvQTv4+qDH77QF0/YLOJFxkREVM4Rhvt99kI/tN32U/o0U7SsNVB2aBeQKBgQC9K8EJ9WQmxq5An/xw76PJRsHkJmbtbqBs2AvSyU+L29vISG+ShJZcF/OOrhS3k0KDNu0tK6lKAFoZ5HAArgImDapsTosTub4BLDQnN/PW/F8mzpQRwgk3ZRGe5q+3e2SwBPMek5OLnpqWxomfxnR7fx0BIfcagDN3Lj3ATiV/gQKBgQCTLEZo2P5nWCHmSVUv0CRcFvO8QgNI2IIhF9JoDA7TBQ56ErAonVKGyATEuylDGvnXBP4wGPh/AyTrYMvT0h2Wh9f+/n7IGQAtG6c6rHyx0Vk8jyLieuPRlFbvTR+3XTDrLes+RkPN/4tmSygxng2xgOm43EFOixcCN8Ba2GIdCw==-----END PRIVATE KEY-----"; // Downloaded from https://console.developers.google.com
        DriveService _gDriveService;
        private ElementReference uploadedFiles;
        private ElementReference outputModal;
        private IEnumerable<IFileReference> selectedFiles;
        private string uploadStatus = string.Empty;
        public System.Threading.CancellationTokenSource cancellationTokenSource;
        string Output { get; set; }
        private static string nl = "<br>";
        public long max;
        public long value;
        public bool CanCancel { get; set; }
        public bool IsCancelDisabled => !CanCancel;
        private SubmissionDto submission;
        private string modalClass = string.Empty;
        private string modalDisplay = "none";
        private bool showBackdrop = false;
        private int selectedPersonIndex = 0;
        private List<UploadFile> filesToUpload;
        private int submittedCount = 0;

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }


        public Index()
        {
            _gDriveService = GetGoogleDriveService();

            var submissionId = ObjectId.GenerateNewId();
            submission = new SubmissionDto {Id = submissionId.ToString(), SubmittedBy = new Person()};
            submission.FeaturedPeople.Add(new Person());
            submission.Event = new Event();
            submission.SubmittedFiles = new List<StoryFile>();
            filesToUpload = new List<UploadFile>();
        }

        protected override async Task OnInitializedAsync()
        {
            var name = await GetStringFromLocalStorage("name");
            if (!string.IsNullOrEmpty(name))
            {
                submission.SubmittedBy.Name = name;
            }

            var email = await GetStringFromLocalStorage("email");
            if (!string.IsNullOrEmpty(email))
            {
                submission.SubmittedBy.Email = email;
            }

            var defaultFeatured = submission.FeaturedPeople.FirstOrDefault();
            var featuredName = await GetStringFromLocalStorage("featuredName");
            if (!string.IsNullOrEmpty(featuredName))
            {
                defaultFeatured.Name = featuredName;
            }

            var featuredClass = await GetStringFromLocalStorage("featuredClass");
            if (!string.IsNullOrEmpty(featuredClass))
            {
                defaultFeatured.ClassOfYear = Convert.ToInt16(featuredClass);
            }
        }

        [Parameter] 
        public EventCallback OnFileRemoved { get; set; }

        protected async Task HandleSelection()
        {
            ShowModalWindow();

            var submissionDateTime = DateTime.UtcNow;
            submission.CreatedAt = submissionDateTime;

            //selectedFiles = await FileReaderService.CreateReference(uploadedFiles).EnumerateFilesAsync();
            Parallel.ForEach(filesToUpload, async (currentFile) =>
            {
                await LoadFile(currentFile);
            });
        }

        protected async Task OnFileInputChange()
        {
            selectedFiles = await FileReaderService.CreateReference(uploadedFiles).EnumerateFilesAsync();
            foreach (var file in selectedFiles)
            {
                var fileInfo = await file.ReadFileInfoAsync();
                max = fileInfo.Size;

                var uploadFile = new StoryFile { Title = fileInfo.Name, Size = fileInfo.Size, Type = fileInfo.Type };
                
                await using (var stream = await file.CreateMemoryStreamAsync())
                { 
                    uploadFile.ImageData = $"data:{fileInfo.Type};base64,{Convert.ToBase64String(stream.ToArray())}";
                }

                submission.SubmittedFiles.Add(uploadFile);
                filesToUpload.Add(new UploadFile(file, uploadFile));
                this.StateHasChanged();
            }
        }

        protected void AddFeaturedPerson()
        {
            submission.FeaturedPeople.Add(new Person());
        }

        protected void RemoveFeaturedPerson(int index)
        {
            submission.FeaturedPeople.RemoveAt(index);
        }

        protected async Task RemoveFile(StoryFile file)
        {
            submission.SubmittedFiles.Remove(file);
            var fileToRemove = filesToUpload.SingleOrDefault(f => f.StoryFile == file);
            filesToUpload.Remove(fileToRemove);
            await FileReaderService.CreateReference(uploadedFiles).ClearValue();
            await OnFileRemoved.InvokeAsync(file);
        }

        protected async Task LoadFile(UploadFile file)
        {
            var fileInfo = await file.FileReference.ReadFileInfoAsync();
            max = fileInfo.Size;

            //var uploadFile = new UploadFile() {Name = fileInfo.Name, Percentage = 0};

            //filesToUpload.Add(uploadFile);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (filesToUpload.Count > 1)
            {
                Output += nl;
            }

            //uploadFile.Size = fileInfo.Size;

            //Output += $"File Name: {fileInfo.Name}{nl}";
            //Output += $"File Size: {fileInfo.Size}{nl}";
            //Output += $"File Type: {fileInfo.Type}{nl}";
            //Output += $"LastModifiedDate: {fileInfo.LastModifiedDate?.ToString() ?? "(N/A)"}{nl}";
            foreach (var property in fileInfo.NonStandardProperties.Keys)
            {
                Output += $"{nameof(IFileInfo)}.{property} (nonstandard): {fileInfo.NonStandardProperties[property]}{nl}";
            }

            this.StateHasChanged();
            Console.WriteLine(Output);
            
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new System.Threading.CancellationTokenSource();
            CanCancel = true;

            const int onlyReportProgressAfterThisPercentDelta = 10;

            // Subscribe to progress (change of position)
            fileInfo.PositionInfo.PositionChanged += (s, e) =>
            {
                // (optional) Only report progress in console / progress bar if percentage has moved over 10% since last call to Acknowledge()
                if (e.PercentageDeltaSinceAcknowledge > onlyReportProgressAfterThisPercentDelta)
                {
                    stopwatch.Stop();
                    // Output += $"Read {(e.PositionDeltaSinceAcknowledge)} bytes ({e.Percentage:00}%). {e.Position} / {fileInfo.Size}{nl}";
                    file.Percentage = e.Percentage;

                    this.InvokeAsync(this.StateHasChanged);
                    e.Acknowledge();
                    value = e.Position;
                    stopwatch.Start();
                }
            };

            Google.Apis.Drive.v3.Data.File newFile = new Google.Apis.Drive.v3.Data.File
            {
                Name = file.StoryFile.Title,
                Description = file.StoryFile.Description,
                MimeType = fileInfo.Type,
                Parents = new List<string>() { driveId }
            };

            await using var fileStream = await file.FileReference.OpenReadAsync();
            var request = _gDriveService.Files.Create(newFile, fileStream, fileInfo.Type);
            request.ProgressChanged += Upload_ProgressChanged;
            request.ResponseReceived += Upload_ResponseReceived;
            request.Fields = "id, name, webViewLink, webContentLink, thumbnailLink, createdTime, size";

            //request.ChunkSize = 1024 * 256 * 4 * 5;


            await request.UploadAsync();

            //await DisplayFilesFromGoogleDrive();
        }

        private string GetContentType(string fileName)
        {
            if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
            {
                return "image/jpeg";
            }

            if (fileName.EndsWith(".png"))
            {
                return "image/png";
            }

            if (fileName.EndsWith(".mp4") || fileName.EndsWith(".m4a") || fileName.EndsWith(".m4p") ||
                fileName.EndsWith(".m4v"))
            {
                return "video/mp4";
            }

            return "application/octet-stream";
        }

        private async void Upload_ResponseReceived(File file)
        {
            var uploadFile = filesToUpload.FirstOrDefault(f => f.StoryFile.Title.Equals(file.Name));
            uploadFile.Percentage = 100;

            uploadStatus = $"{file.Name} was uploaded successfully";
            //Output += $"{uploadStatus} {nl}";
            Console.WriteLine(uploadStatus);
            this.StateHasChanged();

            await this.AddFileToSubmission(file);
        }

        private void Upload_ProgressChanged(IUploadProgress progress)
        {
            uploadStatus = $"{progress.Status} {progress.BytesSent}";
            //Output += $"{uploadStatus} {nl}";
            Console.WriteLine(uploadStatus);
            this.StateHasChanged();
        }

        private void ShowModalWindow()
        {
            modalDisplay = "block";
            modalClass = "Show";
            showBackdrop = true;
            this.StateHasChanged();
        }

        private void CloseModalWindow()
        {
            modalDisplay = "none";
            modalClass = "";
            showBackdrop = false;
            this.StateHasChanged();
        }

        private DriveService GetGoogleDriveService()
        {
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

        private async Task DisplayFilesFromGoogleDrive()
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = _gDriveService.Files.List();
            listRequest.Q = "'" + driveId + "' in parents";
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, filesToUpload(id, name)";

            // List filesToUpload.
            var req = await listRequest.ExecuteAsync();
            IList<Google.Apis.Drive.v3.Data.File> files = req.Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var f in files)
                {
                    Console.WriteLine("{0} ({1})", f.Name, f.Id);
                }
            }
            else
            {
                Console.WriteLine("No filesToUpload found.");
            }
        }

        private async Task AddFileToSubmission(File file)
        {
            var submittedFile = submission.SubmittedFiles.SingleOrDefault(f => f.Title == file.Name);
            submittedFile.DownloadUrl = file.WebContentLink;
            submittedFile.ThumbnailUrl = file.ThumbnailLink;
            submittedFile.CreatedAt = file.CreatedTime.GetValueOrDefault();
            submittedFile.UploadedAt = submission.CreatedAt;

            submittedCount++;

            if (submission.SubmittedFiles.Count == submittedCount)
            {
                await this.AddSubmissionAsync();
            }
        }

        private async Task AddSubmissionAsync()
        {
            await SaveStringToLocalStorage("name", submission.SubmittedBy.Name);
            await SaveStringToLocalStorage("email", submission.SubmittedBy.Email);
            var featuredPerson = submission.FeaturedPeople.FirstOrDefault();
            if (featuredPerson != null)
            {
                await SaveStringToLocalStorage("featuredName", featuredPerson.Name);
                await SaveStringToLocalStorage("featuredClass", featuredPerson.ClassOfYear.ToString());
            }

            await Http.PostAsJsonAsync("api/submissions", submission);
        }

        private async Task SaveStringToLocalStorage(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            await LocalStorage.SetItemAsync<string>(key, value);
        }

        private async Task<string> GetStringFromLocalStorage(string key)
        {
            return await LocalStorage.GetItemAsync<string>(key);
        }
    }
}
