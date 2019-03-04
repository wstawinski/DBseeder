using Couchbase;
using DBseeder.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace DBseeder.EntitySeeders
{
    static class OrderStatusesSeeder
    {
        private static readonly string[] names = { "New", "In progress", "Ready", "Released", "Pending", "Cancelled" };

        private const string chars = "abcdefghijklmnoprstuwxyz";
        private static readonly Random random = new Random();


        public static void Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var mongoCollection = mongoDatabase.GetCollection<OrderStatus>("orderStatusses");
            mongoCollection.DeleteMany(new BsonDocument());

            var couchbaseBucket = couchbaseCluster.OpenBucket("orderStatusses");
            couchbaseBucket.CreateManager().Flush();

            for (int i = 0; i < names.Length; i++)
            {
                var descriptionWordsCount = random.Next(50, 101);
                var description = "";

                for (int j = 0; j < descriptionWordsCount; j++)
                {
                    var wordLength = random.Next(3, 11);
                    description += new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray()) + " ";
                }

                var status = new OrderStatus
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = names[i],
                    Description = description,
                    NumberInSequence = i
                };

                mongoCollection.InsertOne(status);
                couchbaseBucket.Insert(status.Id, status);
            }
        }
    }
}
