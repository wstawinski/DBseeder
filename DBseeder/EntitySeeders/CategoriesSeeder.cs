using Couchbase;
using DBseeder.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBseeder.EntitySeeders
{
    static class CategoriesSeeder
    {
        private static readonly string[] main = { "Notebooks", "Desktops", "RTV", "Household Goods", "Phones", "Cameras", "Gaming", "Office" };
        private static readonly string[] secondary = { "Accessories", "Components", "TVs", "Kitchenware",
            "Smartphones", "Photographic equipment", "Video games", "Office devices" };
        private static readonly string[] tertiary = { "Bags", "Processors", "FullHD TVs", "Microwaves",
            "Android", "Tripods", "PC games", "Faxes" };

        private const string chars = "abcdefghijklmnoprstuwxyz";
        private static readonly Random random = new Random();


        public static void Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var mongoCollection = mongoDatabase.GetCollection<Category>("categories");
            mongoCollection.DeleteMany(new BsonDocument());

            var couchbaseBucket = couchbaseCluster.OpenBucket("categories");
            couchbaseBucket.CreateManager().Flush();

            for (int i = 0; i < main.Length; i++)
            {
                var mainCategory = new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = main[i],
                    ParentCategoryId = Guid.Empty.ToString(),
                    CategoryPath = new List<string>()
                };

                mongoCollection.InsertOne(mainCategory);
                couchbaseBucket.Insert(mainCategory.Id.ToString(), mainCategory);

                var secondaryCategoryPath = new List<string>(mainCategory.CategoryPath);
                secondaryCategoryPath.Add(mainCategory.Name);

                for (int j = 0; j < main.Length; j++)
                {
                    var secondaryCategory = new Category
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentCategoryId = mainCategory.Id,
                        CategoryPath = secondaryCategoryPath
                    };

                    if (j == 0)
                        secondaryCategory.Name = secondary[i];
                    else
                    {
                        var wordLength = random.Next(5, 16);
                        secondaryCategory.Name = new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray());
                    }

                    mongoCollection.InsertOne(secondaryCategory);
                    couchbaseBucket.Insert(secondaryCategory.Id.ToString(), secondaryCategory);

                    var tertiaryCategoryPath = new List<string>(secondaryCategory.CategoryPath);
                    tertiaryCategoryPath.Add(secondaryCategory.Name);

                    for (int k = 0; k < main.Length; k++)
                    {
                        var tertiaryCategory = new Category
                        {
                            Id = Guid.NewGuid().ToString(),
                            ParentCategoryId = secondaryCategory.Id,
                            CategoryPath = tertiaryCategoryPath
                        };

                        if (j == 0 && k == 0)
                            tertiaryCategory.Name = tertiary[i];
                        else
                        {
                            var wordLength = random.Next(5, 16);
                            tertiaryCategory.Name = new string(Enumerable.Repeat(chars, wordLength).Select(s => s[random.Next(s.Length)]).ToArray());
                        }

                        mongoCollection.InsertOne(tertiaryCategory);
                        couchbaseBucket.Insert(tertiaryCategory.Id.ToString(), tertiaryCategory);
                    }
                }
            }
        }
    }
}
