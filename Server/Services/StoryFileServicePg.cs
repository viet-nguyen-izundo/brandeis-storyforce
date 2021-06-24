using StoryForce.Server.Data;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class StoryFileServicePg : DataService<StoryFile>, IStoryFileService
    {
        public StoryFileServicePg(PgDbContext dbContext) : base(dbContext, dbContext.StoryFiles)
        {
        }
    }
}