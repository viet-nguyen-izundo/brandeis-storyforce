using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoryForce.Server.Services;
using StoryForce.Shared.Dtos;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Pages.Admin
{
    public class ShowFileModel : PageModel
    {
        public UserManager<IdentityUser> _userManager;
        public SubmissionService _submissionService;

        public ShowFileModel(UserManager<IdentityUser> userManager, SubmissionService submissionService)
        {
            _userManager = userManager;
            _submissionService = submissionService;
        }

        public IEnumerable<SubmittedByGroup> SubmittedByGroups { get; set; }

        public async Task<ActionResult> OnGetAsync()
        {
            var files = await this._submissionService.GetByRequestedByEmailAsync(_userManager.GetUserName(User));

            SubmittedByGroups = from file in files
                           group file by file.SubmittedBy.Email into submittedBy
                           select new SubmittedByGroup() { SubmittedBy = files.First(x => x.SubmittedBy.Email == submittedBy.Key).SubmittedBy, Files = submittedBy.ToList()};

            return Page();
        }


    }

    public class SubmittedByGroup
    {
        public StoryForce.Shared.Models.Person SubmittedBy { get; set; }

        public List<StoryFile> Files { get; set; }
    }
}
