using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoryForce.Server.Services;

namespace StoryForce.Server.Pages.Admin.Submission.Assignment
{
    public class UserAssignmentFileModel : PageModel
    {
        private IStoryFileAssignmentService _storyFileAssignmentService;

        public UserAssignmentFileModel(IStoryFileAssignmentService storyFileAssignmentService)
        {
            _storyFileAssignmentService = storyFileAssignmentService;

        }
        [BindProperty]
        public StoryForce.Shared.Dtos.StoryFileAssignmentDto StoryFileAssignment { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var Id = string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : 0;
            var assignment = await _storyFileAssignmentService.GetAssignmentById(Id);

            return Page();
        }
    }
}
