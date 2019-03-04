using Couchbase;
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
        private static readonly string[] tvs = { "Philips 32PFT4132", "Philips 43PFS5302", "Philips 43PFT4112", "Samsung UE55NU8002", "Samsung UE55NU7402",
            "Samsung UE55NU7093", "LG 49UK6300", "LG 32LK500B", "LG 49UK6400", "LG 55UK6950" };
        private static readonly string[] pcGames = { "EA FIFA 19", "CENEGA Resident Evil 2", "Techland Metro Exodus", "EA The Sims 4", "CENEGA Fallout 76",
            "EA Battlefield V", "2K Games Cities: Skylines", "CENEGA Sid Meier's Civilization VI", "Rockstar Grand Theft Auto IV", "Rockstar Max Payne 3" };

        private static readonly string[] processorsDetails = { "Family", "Socket", "Clock speed", "Number of cores", "Cache"};
        private static readonly string[] tvsDetails = { "Screen size", "Resolution", "Built-in tuners", "HDMI connectors", "Speakers power"};
        private static readonly string[] pcGamesDetails = { "Digital distribution", "Type", "Age range", "Game modes", "Release date" };

        private const string chars = "abcdefghijklmnoprstuwxyz";
        private static readonly Random random = new Random();


        public static void Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var mongoCollection = mongoDatabase.GetCollection<Product>("products");
            mongoCollection.DeleteMany(new BsonDocument());

            var couchbaseBucket = couchbaseCluster.OpenBucket("products");
            couchbaseBucket.CreateManager().Flush();


            var mongoCategoriesCollection = mongoDatabase.GetCollection<Category>("categories");
            var leafCategoriesMongo = mongoCategoriesCollection.AsQueryable().Where(c => c.CategoryPath.Count == 2).ToList();


            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2019, 1, 1);
            var range = (endDate - startDate).Days;


            for (int i = 0; i < leafCategoriesMongo.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var product = new Product
                    {
                        Id = Guid.NewGuid().ToString(),
                        CategoryId = leafCategoriesMongo[i].Id,
                        Details = new List<ProductDetail>(),
                        Quantity = random.Next(10, 201),
                        ActualPrice = random.Next(50, 2000) + 0.01 * random.Next(0, 100),
                        DateAdded = startDate.AddDays(random.Next(range)).ToString(),
                        PriceHistory = new List<ProductPrice>(),
                        Accessories = new List<ProductAccessory>(),
                        AverageRating = 0,
                        ReviewsCount = 0
                    };

                    if (leafCategoriesMongo[i].Name == "Processors")
                    {
                        product.Name = processors[j];
                        for (int k = 0; k < processorsDetails.Length; k++)
                        {
                            var detail = new ProductDetail
                            {
                                Name = processorsDetails[k],
                                Value = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray())
                            };
                            product.Details.Add(detail);
                        }
                    }
                    else
                        if (leafCategoriesMongo[i].Name == "FullHD TVs")
                        {
                            product.Name = tvs[j];
                            for (int k = 0; k < tvsDetails.Length; k++)
                            {
                                var detail = new ProductDetail
                                {
                                    Name = tvsDetails[k],
                                    Value = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray())
                                };
                                product.Details.Add(detail);
                            }
                        }
                    else
                        if (leafCategoriesMongo[i].Name == "PC games")
                        {
                            product.Name = pcGames[j];
                            for (int k = 0; k < pcGamesDetails.Length; k++)
                            {
                                var detail = new ProductDetail
                                {
                                    Name = pcGamesDetails[k],
                                    Value = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray())
                                };
                                product.Details.Add(detail);
                            }
                        }
                    else
                    {
                        product.Name = new string(Enumerable.Repeat(chars, 50).Select(s => s[random.Next(s.Length)]).ToArray());

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
                    }

                    var descriptionWordsCount = random.Next(100, 201);
                    var description = "";
                    for (int k = 0; k < descriptionWordsCount; k++)
                    {
                        var wordLength = random.Next(3, 11);
                        description += new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray()) + " ";
                    }
                    product.Description = description;

                    var pricesCount = random.Next(4);
                    for (int k = 0; k < pricesCount; k++)
                    {
                        var price = new ProductPrice
                        {
                            Price = random.Next(50, 2000) + 0.01 * random.Next(0, 100),
                            DateStart = startDate.AddDays(random.Next(range)).ToString(),
                            DateEnd = startDate.AddDays(random.Next(range)).ToString()
                        };
                        product.PriceHistory.Add(price);
                    }

                    mongoCollection.InsertOne(product);
                    couchbaseBucket.Insert(product.Id, product);

                }
            }

            var productsMongo = mongoCollection.AsQueryable().ToList();
            var productsCouchbase = new List<Product>();
            foreach (var p in productsMongo)
            {
                var productCouchbase = couchbaseBucket.Get<Product>(p.Id).Value;
                productsCouchbase.Add(productCouchbase);
            }
            for (int i = 0; i < productsMongo.Count; i++)
            {
                var accessoriesCount = random.Next(3, 6);
                for (int j = 0; j < accessoriesCount; j++)
                {
                    var accessoryProduct = productsMongo[random.Next(productsMongo.Count)];
                    var accessory = new ProductAccessory
                    {
                        ProductId = accessoryProduct.Id,
                        Name = accessoryProduct.Name,
                        ActualPrice = accessoryProduct.ActualPrice
                    };
                    productsMongo[i].Accessories.Add(accessory);
                    productsCouchbase[i].Accessories.Add(accessory);
                }
                var filter = Builders<Product>.Filter.Eq(p => p.Id, productsMongo[i].Id);
                mongoCollection.ReplaceOne(filter, productsMongo[i]);
                couchbaseBucket.Replace(productsCouchbase[i].Id, productsCouchbase[i]);
            }
        }
    }
}
