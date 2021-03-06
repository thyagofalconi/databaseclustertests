﻿using System.Linq;
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
            const string collectionName = "BIG_COLLECTION";

            var databaseRepository = new MongoDbRepository(connectionString, databaseName);
            
            //When

            var result = await databaseRepository.ExecuteQuery<MyModel>(collectionName);

            //Then

            Assert.IsTrue(result.Any());
        }

        public class MyModel
        {
            [BsonId]
            public object Id { get; set; }
        }
    }
}
