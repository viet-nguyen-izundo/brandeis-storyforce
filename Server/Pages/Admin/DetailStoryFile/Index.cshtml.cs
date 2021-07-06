using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using StoryForce.Server.Services;
using StoryForce.Shared.Dtos;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Pages.Admin
{
    [Authorize]
    public class StoryFileDetail : PageModel
    {
        private readonly IStoryFileService _storyFileService;
        private readonly IPeopleService _peopleService;

        public StoryFileDetail(IStoryFileService storyFileService, IPeopleService peopleService)
        {
            this._storyFileService = storyFileService;
            this._peopleService = peopleService;
            StoryFile = new();
        }

        [BindProperty]
        public StoryForce.Shared.Models.StoryFile StoryFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            StoryFile = await this._storyFileService.GetAsync(id);
            var listPerson = await this._peopleService.GetByAllAsync();
            //RequestByPerson = listPerson.FindAll(m=>m.Type == 2);
            return Page();
        }

        [BindProperty]
        public StoryForce.Shared.Models.StoryFile File { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (File.Id > 0)
            {
                var file = await _storyFileService.GetAsync(File.Id);
                file.Description = StoryFile.Description;
                //file.Email = User.Email;
                //file.ClassOfYear = User.ClassOfYear;                
                await _storyFileService.UpdateAsync(file.Id, file);
            }
            return Redirect("");
        }

        [BindProperty]
        public IList<StoryForce.Shared.Models.Person> RequestByPerson { get; set; }
        //public async Task<IActionResult> OnGetAsync()
        //{
        //    var listPerson = await this._peopleService.GetAsync();
        //    RequestByPerson = (IList<StoryForce.Shared.Models.Person>)listPerson.Select(m => m.Type = (PersonType?)2);
        //    return Page();
        //}
    }
}
