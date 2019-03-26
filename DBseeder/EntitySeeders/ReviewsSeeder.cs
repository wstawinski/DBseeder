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

            var couchbaseReviews = couchbaseBucket.Query<Review>().ToList();
            foreach (var r in couchbaseReviews)
                couchbaseBucket.Remove(r);

            //Products
            var mongoProductsCollection = mongoDatabase.GetCollection<Product>("products");
            var mongoProducts = mongoProductsCollection.AsQueryable().ToList();

            var couchbaseProducts = couchbaseBucket.Query<Product>().ToList();

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
                        Id = Guid.NewGuid(),
                        Type = "Review",
                        ProductId = mongoProducts[i].Id,
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
                    for (int k = 0; k < textWordsCount; k++)
                    {
                        var wordLength = random.Next(3, 11);
                        text += new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray()) + " ";
                    }
                    review.Text = text;

                    mongoCollection.InsertOne(review);
                    couchbaseBucket.Save(review);
                }

                if (reviewsCount != 0)
                {
                    var couchbaseProduct = couchbaseProducts.Where(p => p.Id == mongoProducts[i].Id).First();

                    mongoProducts[i].ReviewsCount = reviewsCount;
                    couchbaseProduct.ReviewsCount = reviewsCount;

                    var productReviews = mongoCollection.AsQueryable().Where(r => r.ProductId == mongoProducts[i].Id).ToList();
                    var productReviewsRatingSum = 0;
                    foreach (var r in productReviews)
                    {
                        productReviewsRatingSum += r.Rating;
                    }
                    mongoProducts[i].AverageRating = (int)Math.Round((double)productReviewsRatingSum / reviewsCount);
                    couchbaseProduct.AverageRating = (int)Math.Round((double)productReviewsRatingSum / reviewsCount);

                    var filter = Builders<Product>.Filter.Eq(p => p.Id, mongoProducts[i].Id);
                    mongoProductsCollection.ReplaceOne(filter, mongoProducts[i]);
                    couchbaseBucket.Save(couchbaseProduct);
                }
            }
        }
    }
}
