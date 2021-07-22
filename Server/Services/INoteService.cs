using System.Threading.Tasks;
using StoryForce.Server.Controllers;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public interface INoteService : IDataService<Note>
    {
        StoryLogHistory GetNoteDescByCreatedAt(StoryFile storyFile, NoteLogHistory noteLog);
    }
}