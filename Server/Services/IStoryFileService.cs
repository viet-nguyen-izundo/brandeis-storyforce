﻿using System.Collections.Generic;
using System.Threading.Tasks;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public interface IStoryFileService : IDataService<StoryFile>
    {
        Task<List<StoryFile>> GetByRequestedEmailAsync(string email);
        Task<List<StoryFile>> GetBySubmittedByIdAsync(int submittedId);
        Task<List<StoryFile>> GetByStoryFileByInputValueAsync(string value);        
        IList<StoryFile> GetByUserIdAsync(int userId);
        Task UpdateHistoryLog();
    }
}