using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IStoryFileService _storyFileService;
        private readonly ISubmissionService _submissionService;

        public NoteController(INoteService noteService, IStoryFileService storyFileService, ISubmissionService submissionService)
        {
            _noteService = noteService;
            this._storyFileService = storyFileService;
            this._submissionService = submissionService;
        }

        // GET: api/<Note>
        [HttpGet]
        public async Task<List<Note>> Get()
        {
            return await _noteService.GetAsync();
        }

        // GET api/<Note>/5
        [HttpGet("{id}", Name = "GetNote")]
        public async Task<ActionResult<Note>> Get(int id)
        {
            return await _noteService.GetAsync(id);
        }

        // POST api/<Note>
        [HttpPost]
        public async Task<ActionResult> Post(CreateNoteDto note)
        {
            var createdNote = new Note();
            if (note.Text == null)
            {
                return BadRequest("Text null");
            }

            createdNote = await _noteService.CreateAsync(note.ToEntity());
            var storyHistoryLog = new NoteLogHistory();
            var userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (note.SubmissionId != 0)
            {
                var submission = await _submissionService.GetAsync(note.SubmissionId);
                if (submission == null)
                    return BadRequest($"Submission with id '{note.SubmissionId}' not found.");
                submission.NoteFile.Add(createdNote);
                await _submissionService.UpdateAsync(submission.Id, submission);
            }
            else
            {
                var storyFile = await _storyFileService.GetAsync(note.StoryFileId);
                if (storyFile == null)
                    return BadRequest($"Story file with id '{note.StoryFileId}' not found.");
                storyFile.Notes.Add(createdNote);
                await _storyFileService.UpdateAsync(storyFile.Id, storyFile);

                #region Add history Note
                var noteLog = new NoteLogHistory
                {
                    UserId = userId,
                    UserName = User.Identity?.Name,
                    Action = NoteLogAction.Create,

                };
                var storyFileIdUpdated = await _storyFileService.GetAsync(storyFile.Id);
                var newNote = _noteService.GetNoteDescByCreatedAt(storyFileIdUpdated, noteLog);

                if (string.IsNullOrEmpty(storyFileIdUpdated.StoryHistoryLog))
                {
                    var historyJson = Newtonsoft.Json.JsonConvert.SerializeObject(newNote);
                    storyFileIdUpdated.StoryHistoryLog = historyJson;
                }
                else
                {
                    StoryLogHistory historyLog = Newtonsoft.Json.JsonConvert.DeserializeObject<StoryLogHistory>(storyFileIdUpdated.StoryHistoryLog);
                    if (historyLog != null)
                    {
                        historyLog.lstNoteLogHistory.Add(newNote.lstNoteLogHistory[0]);
                        var historyJson = Newtonsoft.Json.JsonConvert.SerializeObject(historyLog);
                        storyFileIdUpdated.StoryHistoryLog = historyJson;
                    }
                }

                await _storyFileService.UpdateAsync(storyFile.Id, storyFileIdUpdated);
                #endregion

            }

            return CreatedAtRoute("GetNote", new { id = createdNote.Id }, createdNote);
        }

        // PUT api/<Note>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, EditNoteDto note)
        {
            var noteInDb = await _noteService.GetAsync(id);
            if (noteInDb == null || note.Id != id)
                return BadRequest($"Note with id '{id}' not found.");


            #region Add history Note

            var noteLog = new NoteLogHistory
            {
                UserId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                UserName = User.Identity?.Name,
                Action = NoteLogAction.Update,
                NoteId = note.Id,
                NoteContent = note.Text,
                OldNoteContent = noteInDb.Text,
                StoryFieldId = note.SubmittedFileId.ToString(),
                CreatedDate = DateTime.Now
            };

            var storyFile = await _storyFileService.GetAsync(note.SubmittedFileId);
            StoryLogHistory historyLog = Newtonsoft.Json.JsonConvert.DeserializeObject<StoryLogHistory>(storyFile.StoryHistoryLog);
            if (historyLog != null)
            {
                historyLog.lstNoteLogHistory.Add(noteLog);
                var historyJson = Newtonsoft.Json.JsonConvert.SerializeObject(historyLog);
                storyFile.StoryHistoryLog = historyJson;
            }

            #endregion
            noteInDb.Text = note.Text;
            await _noteService.UpdateAsync(id, noteInDb);
            await _storyFileService.UpdateAsync(note.SubmittedFileId, storyFile);
            return NoContent();
        }

        // DELETE api/<Note>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, int submittedFileId)
        {
            var noteInDb = await _noteService.GetAsync(id);
            if (noteInDb == null)
                return BadRequest($"Note with id '{id}' not found.");
            #region Add history Note

            var noteLog = new NoteLogHistory
            {
                UserId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                UserName = User.Identity?.Name,
                Action = NoteLogAction.Delete,
                NoteId = id,
                NoteContent = noteInDb.Text,
                OldNoteContent = noteInDb.Text,
                StoryFieldId = submittedFileId > 0 ? submittedFileId.ToString() : string.Empty,
                CreatedDate = DateTime.Now
            };

            if (submittedFileId > 0)
            {
                var storyFile = await _storyFileService.GetAsync(submittedFileId);
                StoryLogHistory historyLog = Newtonsoft.Json.JsonConvert.DeserializeObject<StoryLogHistory>(storyFile.StoryHistoryLog);
                if (historyLog != null)
                {
                    historyLog.lstNoteLogHistory.Add(noteLog);
                    var historyJson = Newtonsoft.Json.JsonConvert.SerializeObject(historyLog);
                    storyFile.StoryHistoryLog = historyJson;
                    await _storyFileService.UpdateAsync(submittedFileId, storyFile);
                }
            }


            #endregion
            await _noteService.RemoveAsync(id);

            return NoContent();
        }
    }

    public class CreateNoteDto
    {
        public int SubmissionId { get; set; }

        public int StoryFileId { get; set; }

        public string Text { get; set; }

        public string Username { get; set; }

        public Note ToEntity()
        {
            return new Note()
            {
                Text = this.Text,
                UserName = this.Username,
                CreatedAt = DateTime.Now,
            };
        }
    }

    public class EditNoteDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int SubmittedFileId { get; set; }
    }

    public class StoryLogHistory
    {
        public List<NoteLogHistory> lstNoteLogHistory { get; set; }
    }

    public class NoteLogHistory
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public NoteLogAction Action { get; set; }
        public int NoteId { get; set; } = 0;
        public string NoteContent { get; set; }
        public string OldNoteContent { get; set; }
        public string SubmissionId { get; set; }
        public string StoryFieldId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public enum NoteLogAction
    {
        Create = 0,
        Update = 1,
        Delete = 2

    }
}
