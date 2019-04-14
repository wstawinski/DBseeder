using Couchbase;
using Couchbase.Linq;
using DBseeder.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBseeder.EntitySeeders
{
    static class ReviewsSeeder
    {
        private const string chars = "abcdefghijklmnoprstuwxyz";
        private static readonly Random random = new Random();

        public static void Seed(IMongoDatabase mongoDatabase, BucketContext couchbaseBucket)
        {
            var mongoCollection = mongoDatabase.GetCollection<Review>("reviews");
            mongoCollection.DeleteMany(new BsonDocument());

            //Products
            var mongoProductsCollection = mongoDatabase.GetCollection<Product>("products");
            var mongoProducts = mongoProductsCollection.AsQueryable().Select(p => p.Id).ToList();

            //Users
            var mongoUsersCollection = mongoDatabase.GetCollection<User>("users");
            var mongoUsers = mongoUsersCollection.AsQueryable().Select(u => new { u.Id, u.Username }).ToList();

            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2020, 1, 1);
            var range = (endDate - startDate).Days;

            for (int i = 0; i < mongoProducts.Count; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    var review = new Review
                    {
                        Id = Guid.NewGuid(),
                        Type = "Review",
                        ProductId = mongoProducts[i],
                        Rating = random.Next(11),
                        DateAdded = DateTime.SpecifyKind(startDate.AddDays(random.Next(range)), DateTimeKind.Utc),
                        HelpfulVotes = random.Next(51),
                        UnhelpfulVotes = random.Next(51)
                    };

                    var user = mongoUsers[random.Next(mongoUsers.Count)];
                    review.UserId = user.Id;
                    review.Username = user.Username;

                    var textWordsCount = random.Next(10, 101);
                    var text = "";
                    for (int l = 0; l < textWordsCount; l++)
                    {
                        var wordLength = random.Next(3, 11);
                        text += new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray()) + " ";
                    }
                    review.Text = text;

                    mongoCollection.InsertOne(review);
                    couchbaseBucket.Save(review);
                }
            }
        }
    }
}
