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

        public IList<StoryFile> storyFile { get; set; }     
        
        public async Task<ActionResult> OnGetAsync()
        {            
            var files = await this._submissionService.GetAsyncbyEmail(_userManager.GetUserName(User));            
            storyFile = files.OrderByDescending(x=>x.CreatedAt).ToList();            
            return Page();
        }

    }
}
