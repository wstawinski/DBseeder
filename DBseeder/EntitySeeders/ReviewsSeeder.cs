using Couchbase;
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

        public static void Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var mongoCollection = mongoDatabase.GetCollection<Review>("reviews");
            mongoCollection.DeleteMany(new BsonDocument());

            var couchbaseBucket = couchbaseCluster.OpenBucket("reviews");
            couchbaseBucket.CreateManager().Flush();

            //Products
            var mongoProductsCollection = mongoDatabase.GetCollection<Product>("products");
            var couchbaseProductsBucket = couchbaseCluster.OpenBucket("products");

            var mongoProducts = mongoProductsCollection.AsQueryable().ToList();
            var couchbaseProducts = new List<Product>();
            foreach (var p in mongoProducts)
            {
                var couchbaseProduct = couchbaseProductsBucket.Get<Product>(p.Id.ToString()).Value;
                couchbaseProducts.Add(couchbaseProduct);
            }

            //Users
            var mongoUsersCollection = mongoDatabase.GetCollection<User>("users");
            var mongoUsers = mongoUsersCollection.AsQueryable().ToList();

            //Seeding
            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2019, 1, 1);
            var range = (endDate - startDate).Days;

            for (int i = 0; i < mongoProducts.Count; i++)
            {
                var reviewsCount = random.Next(6);
                for (int j = 0; j < reviewsCount; j++)
                {
                    var review = new Review
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = mongoProducts[i].Id,
                        Rating = random.Next(11),
                        DateAdded = startDate.AddDays(random.Next(range)).ToString(),
                        HelpfulVotes = random.Next(51),
                        UnhelpfulVotes = random.Next(51)
                    };

                    var user = mongoUsers[random.Next(mongoUsers.Count)];
                    review.UserId = user.Id;
                    review.Username = user.Username;

                    var textWordsCount = random.Next(10, 101);
                    var text = "";
                    for (int k = 0; k < textWordsCount; k++)
                    {
                        var wordLength = random.Next(3, 11);
                        text += new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray()) + " ";
                    }
                    review.Text = text;

                    mongoCollection.InsertOne(review);
                    couchbaseBucket.Insert(review.Id.ToString(), review);
                }

                if (reviewsCount != 0)
                {
                    mongoProducts[i].ReviewsCount = reviewsCount;
                    couchbaseProducts[i].ReviewsCount = reviewsCount;

                    var productReviews = mongoCollection.AsQueryable().Where(r => r.ProductId == mongoProducts[i].Id).ToList();
                    var productReviewsRatingSum = 0;
                    foreach (var r in productReviews)
                    {
                        productReviewsRatingSum += r.Rating;
                    }
                    mongoProducts[i].AverageRating = (double) productReviewsRatingSum / reviewsCount;
                    couchbaseProducts[i].AverageRating = (double) productReviewsRatingSum / reviewsCount;

                    var filter = Builders<Product>.Filter.Eq(p => p.Id, mongoProducts[i].Id);
                    mongoProductsCollection.ReplaceOne(filter, mongoProducts[i]);
                    couchbaseProductsBucket.Replace(couchbaseProducts[i].Id.ToString(), couchbaseProducts[i]);
                }
            }
        }
    }
}
