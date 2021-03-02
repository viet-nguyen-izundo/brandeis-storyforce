using System;
using System.Collections.Generic;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Story : DatabaseEntity
    {
        public string Title { get; set; }

        public List<StoryFile> Files { get; set; }
    }
}
