using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryForce.Server.Migrations;

namespace StoryForce.Server.Services
{
    public interface IStoryFileAssignment : IDataService<StoryFileAssignment>
    {
        Task<List<StoryFileAssignment>> GetStoryFileAssignment(string email);

    }
}

