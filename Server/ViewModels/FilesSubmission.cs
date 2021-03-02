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

namespace StoryForce.Server.ViewModels
{
    public class FilesSubmission
    {
        public FilesSubmission()
        {
            this.FileMetaDataList = new List<FileMeta>();
            this.FeaturedPeople = new List<Person> { new () };
            this.FormFiles = new List<IFormFile>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Files")]
        public List<IFormFile> FormFiles { get; set; }

        public string GDriveOAuthToken { get; set; }

        public List<FileMeta> FileMetaDataList { get; set; }

        public Event Event { get; set; }

        public List<Person> FeaturedPeople { get; set; }

        [Required]
        public Submitter SubmittedBy { get; set; }

        public Submission ConvertToEntity()
        {
            var files = FormFiles.Select((file, index) => new StoryFile()
            {
                Title = file.FileName, 
                Description = FileMetaDataList[index].Description, 
                Size = file.Length, 
                Type = file.ContentType,
                SubmissionTitle = this.Title,
                SubmissionDescription = this.Description,
                FeaturedPeople = this.FeaturedPeople,
                Event = this.Event
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
