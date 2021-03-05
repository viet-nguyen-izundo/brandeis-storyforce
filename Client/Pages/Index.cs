using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
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
using HeyRed.Mime;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using StoryForce.Shared.ViewModels;
using UploadFile = StoryForce.Shared.ViewModels.UploadFile;

namespace StoryForce.Client.Pages
{
    public partial class Index
    {
        private IEnumerable<IBrowserFile> selectedFiles;
        private string uploadStatus = string.Empty;
        public System.Threading.CancellationTokenSource cancellationTokenSource;
        string Output { get; set; }
        private static string nl = "<br>";
        public long max;
        public long value;
        public bool CanCancel { get; set; }
        public bool IsCancelDisabled => !CanCancel;
        private string modalClass = string.Empty;
        private string modalDisplay = "none";
        private bool showBackdrop = false;
        private int selectedPersonIndex = 0;
        private int submittedCount = 0;
        
        [Inject]
        public IConfiguration Configuration { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }


        public Index()
        {
            this.Submission = new BlazorFilesSubmission();
        }

        public BlazorFilesSubmission Submission { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var name = await GetStringFromLocalStorage("name");
            if (!string.IsNullOrEmpty(name))
            {
                this.Submission.SubmittedBy.Name = name;
            }

            var email = await GetStringFromLocalStorage("email");
            if (!string.IsNullOrEmpty(email))
            {
                this.Submission.SubmittedBy.Email = email;
            }

            var defaultFeatured = this.Submission.FeaturedPeople.FirstOrDefault();
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

            await this.AddSubmissionAsync();
        }

        protected async Task OnFileInputChange(InputFileChangeEventArgs e)
        {
            var format = "image/png";
            selectedFiles = e.GetMultipleFiles(10);
            foreach (var file in selectedFiles)
            {
                var uploadFile = new UploadFile(file) { Title = file.Name, Size = file.Size, MimeType = file.ContentType };
                if (file.ContentType.StartsWith("image/"))
                {
                    var resizedImageFile = await file.RequestImageFileAsync(format,
                        250, 250);
                    var buffer = new byte[resizedImageFile.Size];
                    await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                    uploadFile.PreviewUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                }

                this.Submission.UploadFiles.Add(uploadFile);
                this.StateHasChanged();
            }
        }

        protected void AddFeaturedPerson()
        {
            this.Submission.FeaturedPeople.Add(new Person());
        }

        protected void RemoveFeaturedPerson(int index)
        {
            this.Submission.FeaturedPeople.RemoveAt(index);
        }

        protected async Task RemoveFile(UploadFile file)
        {
            this.Submission.UploadFiles.Remove(file);
            await OnFileRemoved.InvokeAsync(file);
        }

        protected async Task LoadFile(UploadFile file)
        {
            max = file.FileReference.Size;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (this.Submission.UploadFiles.Count > 1)
            {
                Output += nl;
            }

            this.StateHasChanged();
            Console.WriteLine(Output);
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

        private async Task AddSubmissionAsync()
        {
            await SaveStringToLocalStorage("name", this.Submission.SubmittedBy.Name);
            await SaveStringToLocalStorage("email", this.Submission.SubmittedBy.Email);
            var featuredPerson = this.Submission.FeaturedPeople.FirstOrDefault();
            if (featuredPerson != null)
            {
                await SaveStringToLocalStorage("featuredName", featuredPerson.Name);
                await SaveStringToLocalStorage("featuredClass", featuredPerson.ClassOfYear.ToString());
            }

            await Http.PostAsJsonAsync("api/submissions/CreateFromBlazor", this.Submission);
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
