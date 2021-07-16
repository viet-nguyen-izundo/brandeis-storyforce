﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoryForce.Server.Data;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class StoryFileServicePg : DataService<StoryFile>, IStoryFileService
    {
        private readonly PgDbContext _dbContext;

        public StoryFileServicePg(PgDbContext dbContext) : base(dbContext, dbContext.StoryFiles)
        {
            _dbContext = dbContext;
        }

        public override Task<List<StoryFile>> GetAsync()
        {
            return _dbContext.StoryFiles
                .Include(x => x.Events)
                .Include(x => x.Submission)
                .Include(x => x.ApprovedSubmission)
                .Include(x => x.FeaturedPeople)
                .Include(x => x.SubmittedBy)
                .Include(x => x.RequestedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.BelongsTo)
                .Include(x => x.Event)
                .Include(x => x.Categories)
                .Include(x => x.Comments)
                .Include(x => x.Notes)
                .Include(x => x.Tags)
                .Include(x => x.FavouritesPeople)
                .ToListAsync();
        }

        public override Task<StoryFile> GetAsync(int id)
        {
            return _dbContext.StoryFiles
                .Include(x => x.Events)
                .Include(x => x.Submission)
                .Include(x => x.ApprovedSubmission)
                .Include(x => x.FeaturedPeople)
                .Include(x => x.SubmittedBy)
                .Include(x => x.RequestedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.BelongsTo)
                .Include(x => x.Event)
                .Include(x => x.Categories)
                .Include(x => x.Comments)
                .Include(x => x.Notes)
                .Include(x => x.Tags)
                .Include(x => x.FavouritesPeople)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<StoryFile>> GetByRequestedEmailAsync(string email)
            => await _dbContext.StoryFiles
                .Include(x => x.Events)
                .Include(x => x.Submission)
                .Include(x => x.ApprovedSubmission)
                .Include(x => x.FeaturedPeople)
                .Include(x => x.SubmittedBy)
                .Include(x => x.RequestedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.BelongsTo)
                .Include(x => x.Event)
                .Include(x => x.Categories)
                .Include(x => x.Comments)
                .Include(x => x.Notes)
                .Include(x => x.FavouritesPeople)
                .Where(s => s.RequestedBy.Email == email)
                .ToListAsync();

        public async Task<List<StoryFile>> GetByStoryFileByInputValueAsync(string value)
        {
            var list = _dbContext.StoryFiles.Include(x => x.Tags).Where(m => m.Title.ToLower().Contains(value.ToLower())
            || m.Description.ToLower().Contains(value.ToLower())
            || m.Tags.Any(t => t.Name.ToLower().Contains(value.ToLower()))).ToList();

            return list;
        }


        public async Task<List<StoryFile>> GetBySubmittedByIdAsync(int submittedById)
            => await _dbContext.StoryFiles
                .Include(x => x.Events)
                .Include(x => x.Submission)
                .Include(x => x.ApprovedSubmission)
                .Include(x => x.FeaturedPeople)
                .Include(x => x.SubmittedBy)
                .Include(x => x.RequestedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.BelongsTo)
                .Include(x => x.Event)
                .Include(x => x.Categories)
                .Include(x => x.Comments)
                .Include(x => x.Notes)
                .Where(s => s.SubmittedBy.Id == submittedById || s.RequestedBy.Id == submittedById)
                .ToListAsync();

        public IList<StoryFile> GetByUserIdAsync(int userId)
        {
            var list = _dbContext.StoryFiles.Include(m=>m.FavouritesPeople).Where(story => story.FavouritesPeople.Any(x => x.Id == userId)).ToList();
            return list;
        }
    }
}