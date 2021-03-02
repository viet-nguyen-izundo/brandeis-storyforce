using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace StoryForce.Shared.Models
{
    [BsonIgnoreExtraElements]
    public class StoryFile : DatabaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Note> Notes { get; set; }

        public ApprovalStatusEnum Status { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Category> Categories { get; set; }

        public Event Event { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Person> FeaturedPeople { get; set; }

        public List<string> Keywords { get; set; }

        public List<AuditDetail> History { get; set; }

        public string SubmissionId { get; set; }

        public string SubmissionTitle { get; set; }

        public string SubmissionDescription { get; set; }

        public string DownloadUrl { get; set; }

        [BsonIgnore]
        public string GoogleFileId
        {
            get
            {
                if (string.IsNullOrEmpty(this.DownloadUrl))
                {
                    return null;
                }

                var uri = new Uri(this.DownloadUrl);
                var qDictionary = HttpUtility.ParseQueryString(uri.Query);
                return qDictionary["id"];
            }
        }

        public long? Size { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Type { get; set; }

        [BsonIgnore]
        public string ImageData { get; set; }

        public DateTime UploadedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Person SubmittedBy { get; set; }

        public Administrator UpdatedBy { get; set; }

        public List<Story> BelongsTo { get; set; }

        public List<Event> Events { get; set; }

        public string GetResizedImageUrl(int width, int height)
        {
            return $"https://lh3.google.com/u/1/d/{this.GoogleFileId}=w{width}-h{height}-p-k-nu-iv1";
        }

        public string GetFileType()
        {
            var title = Title.ToLower();

            if (title.EndsWith(".jpg") || title.EndsWith(".jpeg"))
            {
                return "image/jpeg";
            }

            if (title.EndsWith(".png"))
            {
                return "image/png";
            }

            if (title.EndsWith(".mp4") || title.EndsWith(".m4a") || title.EndsWith(".m4p") ||
                title.EndsWith(".m4v"))
            {
                return "video/mp4";
            }

            return "Unknown";
        }
    }
}
