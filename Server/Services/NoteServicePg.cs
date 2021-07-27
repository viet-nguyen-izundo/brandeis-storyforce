using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryForce.Server.Controllers;
using StoryForce.Server.Data;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class NoteServicePg : DataService<Note>, INoteService
    {
        private readonly PgDbContext _dbContext;
        public NoteServicePg(PgDbContext dbContext) : base(dbContext, dbContext.Notes)
        {
            _dbContext = dbContext;
        }

        public StoryLogHistory GetNoteDescByCreatedAt(StoryFile storyFile, NoteLogHistory noteLog)
        {
            var note = storyFile.Notes.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
            if (note != null)
            {
                var storyLogHistory = new StoryLogHistory
                {
                    lstNoteLogHistory = new List<NoteLogHistory>
                    {
                        new NoteLogHistory
                        {
                            UserId=noteLog.UserId,
                            UserName = noteLog.UserName,
                            Action = noteLog.Action,
                            NoteId = note.Id,
                            NoteContent = note.Text,
                            StoryFieldId = storyFile.Id.ToString(),
                            CreatedDate= DateTime.Now
                        }
                    }
                };

                return storyLogHistory;
            }
            return new StoryLogHistory();
        }
    }
}