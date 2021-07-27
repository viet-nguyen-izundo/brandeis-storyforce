using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using HeyRed.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoryForce.Shared.Models;

namespace StoryForce.Shared.Services
{
    public class GoogleDriveService
    {
        readonly string[] scopes = { DriveService.Scope.Drive };
        DriveService _gDriveService;
        private WebClient _webClient;
        private readonly IConfiguration _configuration;

        public GoogleDriveService(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._gDriveService = InitGoogleDriveService();
            this._webClient = new WebClient();
        }

        public async Task UploadFile(string fileName, string description, string mimeType, Stream fileStream)
        {
            var driveId = this._configuration.GetSection("Google:Drive:DriveId").Value;
            Google.Apis.Drive.v3.Data.File newFile = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName,
                Description = description,
                MimeType = mimeType,
                Parents = new List<string>() { driveId }
            };

            var request = _gDriveService.Files.Create(newFile, fileStream, mimeType);
            request.Fields = "id, name, webViewLink, webContentLink, thumbnailLink, createdTime, size";

            //request.ChunkSize = 1024 * 256 * 4 * 5;
            await request.UploadAsync();
        }

        public async Task DeleteFile(string fileId)
        {
            var request = _gDriveService.Files.Delete(fileId);
            await request.ExecuteAsync();
        }

        public async Task<Byte[]> GetFileBytes(string fileId)
        {
            var request = _gDriveService.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();
            await request.DownloadAsync(stream);
            return stream.ToArray();
        }

        public void SaveFileByUrl(FileMeta fileMeta)
        {
            this._webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileHandler);
            var downloadUrl = fileMeta.DownloadUrl + (fileMeta.accessToken != null ? "?access_token=" + fileMeta.accessToken : string.Empty);
            this._webClient.DownloadFileAsync(new Uri(downloadUrl), fileMeta.Name, fileMeta);
        }

        private async void DownloadFileHandler(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                await UploadToGoogleDrive(e.UserState as FileMeta);
            }
        }

        private async Task UploadToGoogleDrive(FileMeta fileMeta)
        {
            var downloadUrl = fileMeta.DownloadUrl + (fileMeta.accessToken != null ? "?access_token=" + fileMeta.accessToken : string.Empty);
            var stream = await _webClient.OpenReadTaskAsync(new Uri(downloadUrl));
            var mimeType = MimeTypesMap.GetMimeType(fileMeta.Name);
            await UploadFile(fileMeta.Name, fileMeta.Description, mimeType, stream);
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
