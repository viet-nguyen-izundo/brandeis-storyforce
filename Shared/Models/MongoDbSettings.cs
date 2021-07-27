using System;
using System.Collections.Generic;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class MongoDbDatabaseSettings : IMongoDbDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoDbDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}