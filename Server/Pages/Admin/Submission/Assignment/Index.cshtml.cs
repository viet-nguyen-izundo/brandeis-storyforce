using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoryForce.Server.Services;
using StoryForce.Server.ViewModels;
using StoryForce.Shared.Dtos;

namespace StoryForce.Server.Pages.Admin.Submission.Assignment
{
    public class AssignmentModel : PageModel
    {
        private ISubmissionService _submissionService;
        public AssignmentModel(ISubmissionService submissionService)
        {
            this._submissionService = submissionService;
   
        }

        [BindProperty]
        public StoryForce.Shared.Dtos.SubmissionDto Submission { get; set; }

        private int _submissionId;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            _submissionId = id;
            var submission = await this._submissionService.GetAsync(_submissionId);
            this.Submission = SubmissionDto.ConvertFromEntity(submission);

            return Page();
        }
        //[HttpPost]
        //public async Task<IActionResult> PostAssignment(AssignmentRequestModel request)
        //{
        //    return null;
        //}
    }
}
