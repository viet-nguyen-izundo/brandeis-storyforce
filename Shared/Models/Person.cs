﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Person : DatabaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [Display(Name = "Year of Class")]
        public int? ClassOfYear { get; set; }

        public string AvatarUrl { get; set; }
    }
}
