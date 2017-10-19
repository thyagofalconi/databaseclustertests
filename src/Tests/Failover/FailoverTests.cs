using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DatabaseClusterTests.Repository;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;

namespace DatabaseClusterTests.Tests.Failover
{
    [TestFixture]
    public class FailoverTests
    {
        [Test]
        public async Task Run()
        {
            //Given

            const string databaseName = "DATABASE_NAME";
            const string connectionString = "CLUSTER_CONNECTION_STRING";
            const string collectionName = "SMALL_COLLECTION";
            const int numberOfCalls = 100;
            const int intervalInMilliseconds = 500;

            var databaseRepository = new MongoDbRepository(connectionString, databaseName);

            //When, Then

            //When this below is running, disable node 1. It shouldn't fail the test if your failover configuration is correct and works

            for (var count = 0; count < numberOfCalls; count++)
            {
                var result = await databaseRepository.ExecuteQuery<MyModel>(collectionName);

                Assert.IsNotNull(result);

                Thread.Sleep(intervalInMilliseconds);
            }
        }

        public class MyModel
        {
            [BsonId]
            public object Id { get; set; }
        }
    }
}
