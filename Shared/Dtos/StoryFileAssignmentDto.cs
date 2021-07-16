using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using StoryForce.Shared.Models;

namespace StoryForce.Shared.Dtos
{
    public class StoryFileAssignmentDto
    {
        public string Note { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public static List<StoryFileAssignmentDto> ConvertFromEntityToSelect2Vm(List<StoryFileAssignment> entity)
        {
            var lstAssign = new List<StoryFileAssignmentDto>();
            entity.ForEach(x =>
            {
                var newAssign = new StoryFileAssignmentDto
                {
                    Note=x.Note,
                    Description = x.Description,
                    Title = x.Title
                };
                lstAssign.Add(newAssign);
            });
            return lstAssign;
        }
    }
}
