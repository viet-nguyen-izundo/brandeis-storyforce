using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryForce.Server.Migrations;
using StoryForce.Server.ViewModels;

namespace StoryForce.Server.Services
{
    public interface IStoryFileAssignment : IDataService<StoryFileAssignmentService>
    {
        Task<bool> InsertStoryFileAssignment(AssignmentRequestModel request);

    }
}

