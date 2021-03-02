using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace StoryForce.Shared.Models
{
    public class Submission : DatabaseEntity
    {
        public Submission()
        {
            this.SubmittedFiles = new List<StoryFile>();
            this.FeaturedPeople = new List<Person>();
        }

        public Person SubmittedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        
        public Administrator ReviewedBy { get; set; }

        public Administrator ApprovedBy { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<Person> FeaturedPeople { get; set; }

        public Event Event { get; set; }

        public List<AuditDetail> History { get; set; }

        public List<StoryFile> SubmittedFiles { get; set; }

        public List<StoryFile> ApprovedFiles { get; set; }

        public List<StoryFile> RejectedFiles { get; set; }
    }
}
