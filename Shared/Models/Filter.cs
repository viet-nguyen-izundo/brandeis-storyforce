using System;
using System.Collections.Generic;
using System.Text;

namespace StoryForce.Shared.Models
{
    public abstract class Filter : DatabaseEntity
    {
        public string Name { get; set; }
        public abstract string Type { get; }
    }
}
