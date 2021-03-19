using StoryForce.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoryForce.Server.ViewModels;

namespace StoryForce.Server.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
            Submission = new FilesSubmission();

        }
        [BindProperty]
        public FilesSubmission Submission { get; set; }

        public void OnGet()
        {
            
        }

        public void AddPerson()
        {

        }
    }
}
