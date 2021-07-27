using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StoryForce.Shared.ViewModels;

namespace StoryForce.Client.UI
{
    public class Interop
    {
        private readonly IJSRuntime _jsRuntime;

        public Interop(IJSRuntime jsRuntime)
        {
            this._jsRuntime = jsRuntime;
        }

        public async Task<string[]> UploadFiles(string postUrl, string fileInputId, string[] descriptions, string keyPrefix)
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string[]>(
                    "StoryForce.Interop.uploadFilesToS3", new TimeSpan(1, 0, 0),
                fileInputId, descriptions, keyPrefix);
            }
            catch(Exception err)
            {
                return null;
            }
        }

        public async ValueTask<UploadFile> UploadFileByUrl(string postUrl, string fileUrl, string fileName, string description)
        {
            try
            {
                var uploadedFile = await _jsRuntime.InvokeAsync<UploadFile>(
                    "StoryForce.Interop.uploadFileByUrl",
                    postUrl, fileUrl, fileName, description);
                return uploadedFile;
            }
            catch
            {
                return null;
            }
        }

    }
}
