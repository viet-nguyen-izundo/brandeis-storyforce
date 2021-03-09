using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using MongoDB.Bson;
using StoryForce.Shared.Models;

namespace StoryForce.Shared.ViewModels
{
    public class BlazorFilesSubmission
    {
        public BlazorFilesSubmission()
        {
            this.SubmittedBy = new Person();
            this.FileMetaDataList = new List<FileMeta>();
            this.FeaturedPeople = new List<Person> { new () };
            this.UploadFiles = new List<UploadFile>();
            this.Event = new Event();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Files")]
        [ValidateComplexType]
        public List<UploadFile> UploadFiles { get; set; }

        public string GDriveOAuthToken { get; set; }

        public List<FileMeta> FileMetaDataList { get; set; }

        public Event Event { get; set; }

        [Required]
        [ValidateComplexType]
        public List<Person> FeaturedPeople { get; set; }

        [Required]
        [ValidateComplexType]
        public Person SubmittedBy { get; set; }

        public Submission ConvertToEntity()
        {
            var files = UploadFiles.Select((file, index) => new StoryFile()
            {
                Title = file.Title,
                Description = file.Description,
                Size = file.Size,
                Type = file.MimeType,
                SubmissionTitle = this.Title,
                SubmissionDescription = this.Description,
                FeaturedPeople = this.FeaturedPeople,
                Event = !string.IsNullOrEmpty(this.Event.Name) ? this.Event : null
            }).ToList();

            return new Submission
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = this.Title,
                Description = this.Description,
                SubmittedFiles = files,
                FeaturedPeople = this.FeaturedPeople,
                Event = this.Event,
                SubmittedBy = this.SubmittedBy,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
