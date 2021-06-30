using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoryForce.Server.Services;
using StoryForce.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // GET: api/<Note>
        [HttpGet]
        public async Task<List<Note>> Get()
        {
            return await _noteService.GetAsync();
        }

        // GET api/<Note>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(int id)
        {
            return await _noteService.GetAsync(id);
        }

        // POST api/<Note>
        [HttpPost]
        public async Task<ActionResult> Post(CreateNoteDto note)
        {
            var createdNote = await _noteService.CreateAsync(note.ToEntity());
            return CreatedAtRoute("/api/Note", new { id = createdNote.Id });
        }

        // PUT api/<Note>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, EditNoteDto note)
        {
            var noteInDb = await _noteService.GetAsync(id);
            if (noteInDb == null || note.Id != id)
                return BadRequest($"Note with id '{id}' not found.");
            noteInDb.Text = note.Text;
            await _noteService.UpdateAsync(id, noteInDb);
            return NoContent();
        }

        // DELETE api/<Note>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var noteInDb = await _noteService.GetAsync(id);
            if (noteInDb == null)
                return BadRequest($"Note with id '{id}' not found.");
            await _noteService.RemoveAsync(id);
            return NoContent();
        }
    }

    public class CreateNoteDto
    {
        public string Text { get; set; }

        public string Username { get; set; }

        public Note ToEntity()
        {
            return new Note()
            {
                Text = this.Text,
                UserName =  this.Username,
                CreatedAt = DateTime.Now
            };
        }
    }

    public class EditNoteDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
    }
}
