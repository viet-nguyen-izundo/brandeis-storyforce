using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using StoryForce.Server.Services;
using StoryForce.Shared.Dtos;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Pages.Admin.Submission
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private ISubmissionService _submissionService;
        private readonly IConfiguration _configuration;
        private IAmazonS3 _s3Client;        
        private string _s3BucketName;
        private readonly IPeopleService _peopleService;
        public UserManager<StoryForce.Shared.Models.Person> _userManager;
        public DetailsModel(ISubmissionService submissionService, IConfiguration configuration, IAmazonS3 s3Client,
            UserManager<StoryForce.Shared.Models.Person> userManager, IPeopleService peopleService)
        {
            this._submissionService = submissionService;
            this._configuration = configuration;
            this._s3BucketName = this._configuration.GetSection("AWS:S3:BucketName").Value;
            this._s3Client = s3Client;
            _userManager = userManager;
            this._peopleService = peopleService;
        }      
    
        [BindProperty]
        public StoryForce.Shared.Dtos.SubmissionDto Submission { get; set; }

        [BindProperty]
        public StoryForce.Shared.Models.Person UserIden { get; set; }
        private int _submissionId;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            _submissionId = id;
            var submission = await this._submissionService.GetAsync(_submissionId);
            this.Submission = SubmissionDto.ConvertFromEntity(submission);        
            this.UserIden = await this._peopleService.GetByEmailAsync(_userManager.GetUserName(User));
            return Page();
        }      

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await this._submissionService.RemoveWithFilesAsync(id);
            return new RedirectToPageResult("/Admin/Index");
        }        
    }
}
