using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoryForce.Server.Data;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class INoteService : DataService<Note>
    {
        private readonly PgDbContext _dbContext;

        public INoteService(PgDbContext dbContext) : base(dbContext, dbContext.Notes)
        {
            _dbContext = dbContext;
        }       

        public async Task RemoveWithFilesAsync(int id)
        {           
            try
            {
                var Notes =  _dbContext.Notes.Where(m=>m.Id == id);
                _dbContext.Notes.Remove((Note)Notes);
                await _dbContext.SaveChangesAsync();                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleting Notes: " + e.Message);                
            }
        }
    }

}