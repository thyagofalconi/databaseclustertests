using System;
using System.Threading.Tasks;
using DatabaseClusterTests.Repository;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;

namespace DatabaseClusterTests.Tests.DataReplication
{
    [TestFixture]
    public class DataReplicationTests
    {
        [Test]
        public async Task Run()
        {
            //Given

            const string databaseName = "DATABASE_NAME";
            const string clusterConnectionString = "CLUSTER_CONNECTION_STRING";
            const string node1ConnectionString = "NODE_1_CONNECTION_STRING";
            const string node2ConnectionString = "NODE_2_CONNECTION_STRING";

            var clusterDatabaseRepository = new MongoDbRepository(clusterConnectionString, databaseName);
            var node1DatabaseRepository = new MongoDbRepository(node1ConnectionString, databaseName);
            var node2DatabaseRepository = new MongoDbRepository(node2ConnectionString, databaseName);

            var collectionName = Guid.NewGuid().ToString();

            var objectId = Guid.NewGuid();

            var model = new MyModel { Id = objectId };

            //When, Then
            
            try
            {
                var result = await clusterDatabaseRepository.Upsert(model, collectionName);

                Assert.IsTrue(result);

                var resultNode1 = await node1DatabaseRepository.Get<MyModel>(model.Id, collectionName);

                Assert.IsNotNull(resultNode1);
                Assert.IsTrue(resultNode1.Id == model.Id);

                var resultNode2 = await node2DatabaseRepository.Get<MyModel>(model.Id, collectionName);

                Assert.IsNotNull(resultNode2);
                Assert.IsTrue(resultNode2.Id == model.Id);
            }
            finally
            {
                clusterDatabaseRepository.DeleteCollection(collectionName);
            }
        }

        public class MyModel : IModel
        {
            [BsonId]
            public object Id { get; set; }
        }
    }
}