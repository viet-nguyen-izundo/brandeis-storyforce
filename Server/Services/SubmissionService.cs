using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Services
{
    public class SubmissionService
    {
        private readonly IMongoCollection<Submission> _submissions;
        private readonly IMongoCollection<StoryFile> _storyFiles;
        private readonly MongoClient _client;

        public SubmissionService(IMongoDbDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            var database = _client.GetDatabase(settings.DatabaseName);

            _submissions = database.GetCollection<Submission>("Submissions");
            _storyFiles = database.GetCollection<StoryFile>("StoryFiles");
        }

        public async Task<List<Submission>> GetAsync() =>
            (await _submissions.FindAsync(s => true)).ToList();

        public async Task<Submission> GetAsync(string id) =>
            (await _submissions.FindAsync<Submission>(s => s.Id == id)).FirstOrDefault();

        public async Task<Submission> CreateAsync(Submission submission)
        {
            await _submissions.InsertOneAsync(submission);
            return submission;
        }

        public async Task UpdateAsync(string id, Submission submission) =>
            await _submissions.ReplaceOneAsync(s => s.Id == id, submission);

        public async Task RemoveAsync(Submission submission) =>
            await RemoveAsync(submission.Id);

        public async Task RemoveAsync(string id) =>
            await _submissions.DeleteOneAsync(s => s.Id == id);

        public async Task RemoveWithFilesAsync(string id)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await _storyFiles.DeleteManyAsync(f => f.SubmissionId == id);
                await _submissions.FindOneAndDeleteAsync(s => s.Id == id);

                await session.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleting Submission and its StoryFiles: " + e.Message);
                await session.AbortTransactionAsync();
            }
        }
    }
}
