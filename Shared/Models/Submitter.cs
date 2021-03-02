using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Submitter : Person
    {
        [Required]
        public override string Email { get; set; }
    }
}
