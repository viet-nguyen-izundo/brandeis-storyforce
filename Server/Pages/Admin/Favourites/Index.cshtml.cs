using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using StoryForce.Server.Services;
using StoryForce.Shared.Dtos;

namespace StoryForce.Server.Pages.Admin.Favourites
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IStoryFileService _storyFileService;
        private readonly IAmazonS3 _s3Client;
        private readonly IPeopleService _peopleService;
        public UserManager<StoryForce.Shared.Models.Person> _userManager;

        public IndexModel(UserManager<StoryForce.Shared.Models.Person> userManager, IStoryFileService storyFileService, IPeopleService peopleService)
        {
            this._storyFileService = storyFileService;
            _userManager = userManager;
            this._peopleService = peopleService;
            storyFiles = new List<StoryForce.Shared.Models.StoryFile>();
        }
        public IList<StoryForce.Shared.Models.StoryFile> storyFiles { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this._peopleService.GetByEmailAsync(_userManager.GetUserName(User));
            storyFiles = this._storyFileService.GetByUserIdAsync(user.Id);
            return Page();
        }
    }
}

