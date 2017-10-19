using System.Threading.Tasks;
using DatabaseClusterTests.Repository;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;

namespace DatabaseClusterTests.Tests.Load
{
    [TestFixture]
    public class LoadTests
    {
        [Test]
        public async Task Run()
        {
            //Given

            const string databaseName = "DATABASE_NAME";
            const string connectionString = "CONNECTION_STRING";

            var databaseRepository = new MongoDbRepository(connectionString, databaseName);
            
            //When

            var result = await databaseRepository.ExecuteExpensiveQuery<MyModel>();

            //Then

            Assert.IsTrue(result);
        }

        public class MyModel
        {
            [BsonId]
            public object Id { get; set; }
        }
    }
}
