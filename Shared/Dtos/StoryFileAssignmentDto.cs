using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryForce.Shared.Models;

namespace StoryForce.Shared.Dtos
{
    public class StoryFileAssignmentDto
    {
        public string Note { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public static IEnumerable<StoryFileAssignmentDto> ConvertFromEntityToSelect2Vm(StoryFileAssignment[] entity)
        {
            foreach (var item in entity)
            {
                yield return new StoryFileAssignmentDto
                {
                    Note = item.Note,
                    Description=item.DescriptionStoryFile,
                    Title = item.TitleStoryFile
                };
            }
        }
    }
}
