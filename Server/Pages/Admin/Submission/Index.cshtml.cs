using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoryForce.Server.Services;
using StoryForce.Shared.Dtos;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Pages.Admin.Submission
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private SubmissionService _submissionService;
        public DetailsModel(SubmissionService submissionService)
        {
            this._submissionService = submissionService;
        }

        [BindProperty]
        public StoryForce.Shared.Dtos.SubmissionDto Submission { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var submission = await this._submissionService.GetAsync(id);
            this.Submission = SubmissionDto.ConvertFromEntity(submission);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await this._submissionService.RemoveWithFilesAsync(id);
            return new RedirectToPageResult("/Admin/Index");
        }
    }
}
