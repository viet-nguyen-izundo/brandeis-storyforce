using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Packaging.Ionic.Zip;
using StoryForce.Server.Data;
using StoryForce.Server.ViewModels;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class StoryFileAssignmentService: DataService<StoryFileAssignment>, IStoryFileAssignment
    {
        private readonly PgDbContext _dbContext;
        public StoryFileAssignmentService(PgDbContext dbContext) : base(dbContext, dbContext.StoryFileAssignment)
        {
        }


        public async Task<bool> InsertStoryFileAssignment(AssignmentRequestModel request)
        {
            if (!request.AssignmentFiles.Any())
                return false;
            //request.AssignmentFiles.ToList().ForEach(x =>
            //{
            //    var assignmentFile = new StoryFileAssignment
            //    {
            //        StoryFile=x.StoryFileId;
            //    };
            //});
            return true;
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
