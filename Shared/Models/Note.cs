using StoryForce.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Note : DatabaseEntity
    {
        public string Text { get; set; }
        public string UserName { get; set; }
    }
}
