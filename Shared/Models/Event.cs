using System;
using System.Collections.Generic;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Event : Filter
    {
        public int? Category { get; set; }

        public override string Type { get; } = "Event";

        public DateTime? Date { get; set; }
    }
}
