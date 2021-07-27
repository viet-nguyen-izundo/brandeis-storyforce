using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryForce.Server.ViewModels;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public interface IStoryFileAssignmentService : IDataService<StoryFileAssignmentService>
    {
        Task<bool> InsertStoryFileAssignment(AssignmentRequestModel request);
        Task<List<StoryFileAssignment>> GetAssignmentById(int Id);

    }
}

