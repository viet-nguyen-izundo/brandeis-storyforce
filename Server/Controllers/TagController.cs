using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StoryForce.Server.Services;
using StoryForce.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IStoryFileService _storyFileService;
        private readonly ISubmissionService _submissionService;
        private readonly ITagService _tagsService;

        public TagController(INoteService noteService, IStoryFileService storyFileService, ISubmissionService submissionService, ITagService tagService)
        {
            _noteService = noteService;
            this._storyFileService = storyFileService;
            this._submissionService = submissionService;
            this._tagsService = tagService;
        }

        // GET: api/<Tag>
        [HttpGet]
        public async Task<List<Tag>> Get()
        {
            return await _tagsService.GetAsync();
        }

        // GET api/<Note>/5
        [HttpGet("{id}", Name = "GetTag")]
        public async Task<ActionResult<Tag>> Get(int id)
        {
            return await _tagsService.GetAsync(id);
        }

        // POST api/<Note>
        [HttpPost]
        public async Task<ActionResult> Post(CreateTagDto tagDto)
        {
            var createdTag = await _tagsService.CreateAsync(tagDto.ToEntity());
            //if (tagDto.SubmissionId != 0)
            //{
            //    var submission = await _submissionService.GetAsync(tagDto.SubmissionId);
            //    if (submission == null)
            //        return BadRequest($"Submission with id '{note.SubmissionId}' not found.");
            //    submission.NoteFile.Add(createdNote);
            //    await _submissionService.UpdateAsync(submission.Id, submission);
            //}
            //else
            {
                var storyFile = await _storyFileService.GetAsync(tagDto.StoryFileId);
                if (storyFile == null)
                    return BadRequest($"Story file with id '{tagDto.StoryFileId}' not found.");
                storyFile.Tags.Add(createdTag);
                await _storyFileService.UpdateAsync(storyFile.Id, storyFile);
            }

            return CreatedAtRoute("GetTag", new { id = createdTag.Id }, createdTag);
        }

        //    // PUT api/<Note>/5
        //    [HttpPut("{id}")]
        //    public async Task<ActionResult> Put(int id, EditNoteDto note)
        //    {
        //        var noteInDb = await _tagsService.GetAsync(id);
        //        if (noteInDb == null || note.Id != id)
        //            return BadRequest($"Note with id '{id}' not found.");
        //        noteInDb.Text = note.Text;
        //        await _noteService.UpdateAsync(id, noteInDb);
        //        return NoContent();
        //    }

        //    // DELETE api/<Note>/5
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult> Delete(int id)
        //    {
        //        var noteInDb = await _noteService.GetAsync(id);
        //        if (noteInDb == null)
        //            return BadRequest($"Note with id '{id}' not found.");
        //        await _noteService.RemoveAsync(id);
        //        return NoContent();
        //    }
        //}

        public class CreateTagDto
        {
            public string Name { get; set; }

            public int StoryFileId { get; set; }
            public Tag ToEntity()
            {
                return new Tag()
                {
                    Name = this.Name,
                    CreatedAt = DateTime.Now
                };
            }
        }

        //public class EditNoteDto
        //{
        //    public int Id { get; set; }
        //    public string Text { get; set; }

        //}
    }
}
