using System;
using System.Collections.Generic;
using System.Text;
using StoryForce.Shared.Models;

namespace StoryForce.Shared.Dtos
{
    public class UIDataDto
    {
        public IList<Category> Categories { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}
