using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DatabaseClusterTests.Repository
{
    public class MongoDbRepository
    {
        private readonly IMongoDatabase _database;

        public MongoDbRepository(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);

            _database = client.GetDatabase(databaseName);
        }

        public async Task<bool> ExecuteExpensiveQuery<T>()
        {
            var collection = _database.GetCollection<T>("bigcollection");

            var result = await collection.Find(new BsonDocument()).ToListAsync();

            return true;
        }
    }
}