using Couchbase;
using DBseeder.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace DBseeder.EntitySeeders
{
    static class PaymentMethodsSeeder
    {
        private static readonly string[] names = { "Cash", "Credit card - upon delivery", "Credit card - online",
            "Traditional bank transfer", "Instant bank transfer", "Installment" };

        private const string chars = "abcdefghijklmnoprstuwxyz";
        private static readonly Random random = new Random();


        public static void Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var mongoCollection = mongoDatabase.GetCollection<PaymentMethod>("paymentMethods");
            mongoCollection.DeleteMany(new BsonDocument());

            var couchbaseBucket = couchbaseCluster.OpenBucket("paymentMethods");
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

                var method = new PaymentMethod
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = names[i],
                    Description = description
                };

                mongoCollection.InsertOne(method);
                couchbaseBucket.Insert(method.Id.ToString(), method);
            }
        }
    }
}
