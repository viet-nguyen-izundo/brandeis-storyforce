using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoryForce.Shared.Services
{
    public class FileService
    {
        private WebClient _webClient;

        public FileService()
        {
            this._webClient = new WebClient();
        }

        public void SaveFileByUrl(string url, string fileName, string fileId, string accessToken = null)
        {
            this._webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileHandler);
            var downloadUrl = url + (accessToken != null ? "?access_token=" + accessToken : string.Empty);
            this._webClient.DownloadFileAsync(new Uri(downloadUrl), fileName, fileId);
        }

        private void DownloadFileHandler(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                UploadToGoogleDrive(e.UserState.ToString());
            }
        }

        private void UploadToGoogleDrive(string fileId)
        {

        }
    }
}
