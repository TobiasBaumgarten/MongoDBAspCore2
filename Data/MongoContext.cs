using Microsoft.Extensions.Options;
using Mongo.Models;
using MongoDB.Driver;

namespace Mongo.Data
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Note> Notes
        {
            get => _database.GetCollection<Note>("Note");
        }

        public IMongoCollection<ApplicationUser> Users{
             get  => _database.GetCollection<ApplicationUser>("User");
        }
    }
}