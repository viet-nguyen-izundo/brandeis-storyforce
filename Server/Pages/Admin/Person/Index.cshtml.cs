using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoryForce.Server.Services;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Pages.Admin.Person
{
    public class IndexModel : PageModel
    {
        private PeopleService _peopleService;
        public IndexModel(PeopleService peopleService)
        {
            this._peopleService = peopleService;
        }

        [BindProperty]
        public IEnumerable<StoryForce.Shared.Models.Person> People { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            this.People = await this._peopleService.GetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await this._peopleService.RemoveAsync(id);
            return new RedirectToPageResult("/Admin/Person/Index");
        }
    }
}
