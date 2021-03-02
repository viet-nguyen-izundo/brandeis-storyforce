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
using StoryForce.Shared.Services;

namespace StoryForce.Server.Pages.Admin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private SubmissionService _submissionService;

        public IndexModel(SubmissionService submissionService
            , StoryFileService storyFileService
            , PeopleService peopleService)
        {
            this._submissionService = submissionService;
        }

        public IList<SubmissionDto> Submissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var submissionModels = await this._submissionService.GetAsync();
            Submissions = submissionModels.Select(submission => SubmissionDto.ConvertFromEntity(submission)).OrderByDescending(s => s.CreatedAt).ToList();
            return Page();
        }
    }
}
