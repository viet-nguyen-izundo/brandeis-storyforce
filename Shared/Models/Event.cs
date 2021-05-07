using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Event : Filter
    {
        public int? Category { get; set; }

        public override string Type { get; } = "Event";

        public DateTime? Date { get; set; }

        [Display(Name = "Event Year")]
        [Range(1960, 3000)]

        public int? Year { get; set; }
    }
}
