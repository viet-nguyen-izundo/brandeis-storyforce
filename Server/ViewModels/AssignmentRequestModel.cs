using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryForce.Shared.Models;

namespace StoryForce.Server.ViewModels
{
    public class AssignmentRequestModel
    {

        //public FileStatus FileStatus { get; set; }
        public int AssignedToId { get; set; }
        public IEnumerable<AssessmentChild> AssignmentFiles { get; set; }
    }

    public class AssessmentChild
    {
        public string Note { get; set; }
        public int StoryFileId { get; set; }
    }
}
