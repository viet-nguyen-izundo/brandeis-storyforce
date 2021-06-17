using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Person : DatabaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        [Display(Name = "Year")]
        public int? ClassOfYear { get; set; }

        public string AvatarUrl { get; set; }

        public PersonType? Type { get; set; }
    }

    public enum PersonType
    {
        Student,
        Class,
        Staff,
        Parent,
        Alumni,
        CommunityMember,
        Other
    }
}
