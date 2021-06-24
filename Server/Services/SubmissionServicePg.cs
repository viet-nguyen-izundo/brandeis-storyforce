using StoryForce.Server.Data;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class SubmissionServicePg : DataService<Submission>, ISubmissionService
    {
        private readonly PgDbContext _dbContext;

        public SubmissionServicePg(PgDbContext dbContext) : base(dbContext, dbContext.Submissions)
        {
            _dbContext = dbContext;
        }
        
    }
    
}