using Couchbase;
using Couchbase.Linq;
using DBseeder.Entities;
using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBseeder.EntitySeeders
{
    static class ProductsSeeder
    {
        private static readonly string[] processors = { "Intel i9-9900K 3.6 GHz 16MB BOX", "Intel i7-9700K 3.6GHz 12MB BOX", "Intel i5-9600K 3.7 GHz 9MB BOX",
            "Intel i7-8700 3.20GHz 12MB BOX", "Intel i5-8600K 3.60GHz 9MB", "Intel i5-8400 2.80GHz 9MB BOX", "Intel i3-8100 3.60GHz 6MB BOX",
            "AMD Ryzen 7 2700X", "AMD Ryzen 5 2600", "AMD Ryzen 5 1600X 3.6GHz" };

        private const string chars = "abcdefghijklmnoprstuwxyz";
        private static readonly Random random = new Random();


        public static void Seed(IMongoDatabase mongoDatabase, BucketContext couchbaseBucket)
        {
            var mongoCollection = mongoDatabase.GetCollection<Product>("products");
            mongoCollection.DeleteMany(new BsonDocument());

            var mongoCategoriesCollection = mongoDatabase.GetCollection<Category>("categories");
            var leafCategoriesMongo = mongoCategoriesCollection.AsQueryable().Where(c => c.CategoryPath.Count == 2).Select(c => new { c.Id, c.Name }).ToList();

            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2020, 1, 1);
            var range = (endDate - startDate).Days;

            for (int i = 0; i < leafCategoriesMongo.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Type = "Product",
                        CategoryId = leafCategoriesMongo[i].Id,
                        Details = new List<ProductDetail>(),
                        Quantity = random.Next(10, 201),
                        ActualPrice = (decimal)random.Next(1000, 1000000) / 100,
                        DateAdded = DateTime.SpecifyKind(startDate.AddDays(random.Next(range)), DateTimeKind.Utc),
                        AverageRating = random.Next(11),
                        ReviewsCount = 10
                    };

                    if (leafCategoriesMongo[i].Name == "Processors" && j < 10)
                        product.Name = processors[j];
                    else
                        product.Name = new string(Enumerable.Repeat(chars, 50).Select(s => s[random.Next(s.Length)]).ToArray());

                    var descriptionWordsCount = random.Next(100, 201);
                    var description = "";
                    for (int k = 0; k < descriptionWordsCount; k++)
                    {
                        var wordLength = random.Next(3, 11);
                        description += new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray()) + " ";
                    }
                    product.Description = description;

                    var detailsCount = random.Next(5, 11);
                    for (int k = 0; k < detailsCount; k++)
                    {
                        var detail = new ProductDetail
                        {
                            Name = new string(Enumerable.Repeat(chars, 15).Select(s => s[random.Next(s.Length)]).ToArray()),
                            Value = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray())
                        };
                        product.Details.Add(detail);
                    }

                    mongoCollection.InsertOne(product);
                    couchbaseBucket.Save(product);
                }
            }
        }
    }
}
