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

        public async Task<bool> Upsert<T>(T model, string collectionName) where T : IModel
        {
            var collection = _database.GetCollection<T>(collectionName);

            var filter = Builders<T>.Filter.Eq(x => x.Id, model.Id);

            var options = new FindOneAndReplaceOptions<T, T>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            await collection.FindOneAndReplaceAsync(filter, model, options);

            return true;
        }

        public async Task<T> Get<T>(object modelId, string collectionName) where T : IModel
        {
            var collection = _database.GetCollection<T>(collectionName);

            var filter = Builders<T>.Filter.Eq(x => x.Id, modelId);

            var result = await collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }

        public void DeleteCollection(string collectionName)
        {
            _database.DropCollection(collectionName);
        }
    }
}