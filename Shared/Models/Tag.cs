using System.Collections.Generic;
using StoryForce.Shared.Interfaces;

namespace StoryForce.Shared.Models
{
    public class Tag : Filter, IStoryHistory
    {
        public override string Type { get; } = "Tag";

        public ICollection<StoryFile> StoryFiles { get; set; }
        public string StoryHistoryLog { get; set; }
    }
}
