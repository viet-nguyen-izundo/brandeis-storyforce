using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using OfficeOpenXml.Packaging.Ionic.Zip;
using StoryForce.Server.Data;
using StoryForce.Server.ViewModels;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class StoryFileAssignmentService : DataService<StoryFileAssignment>, IStoryFileAssignmentService
    {
        private readonly PgDbContext _dbContext;
        public StoryFileAssignmentService(PgDbContext dbContext) : base(dbContext, dbContext.StoryFileAssignments)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> InsertStoryFileAssignment(AssignmentRequestModel request)
        {
            if (!request.AssignmentFiles.Any())
                return false;
            request.AssignmentFiles.ToList().ForEach(x =>
            {
                var assignmentFile = new StoryFileAssignment
                {
                    StoryFileId = x.StoryFileId,
                    Note = x.Note.Trim(),
                    AssignedToId = request.AssignedToId,
                    FileStatus = FileStatus.New,
                    Title = x.TitleAssignment.Trim(),
                    Description = x.DescriptionAssignment.Trim()
                };
                _dbContext.StoryFileAssignments.AddAsync(assignmentFile);
            });
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<List<StoryFileAssignment>> GetAssignmentById(int Id)
        {
            var storyfile = await _dbContext.StoryFileAssignments.Where(x => x.AssignedToId.Equals(Id)).ToListAsync();
            return storyfile;
        }

        public Task<List<StoryFileAssignmentService>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StoryFileAssignmentService> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<StoryFileAssignmentService> CreateAsync(StoryFileAssignmentService entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<StoryFileAssignmentService>> CreateMultipleAsync(List<StoryFileAssignmentService> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, StoryFileAssignmentService entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(StoryFileAssignmentService entity)
        {
            throw new NotImplementedException();
        }
    }
}
