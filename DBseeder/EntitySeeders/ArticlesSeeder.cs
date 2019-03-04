using Couchbase;
using DBseeder.Entities;
using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DBseeder.EntitySeeders
{
    class ArticlesSeeder
    {
        private const string apiPath = "https://newsapi.org/v2/everything?pageSize=100";
        private static readonly string[] sources = { "bbc-news", "business-insider-uk", "daily-mail",
            "financial-times", "google-news-uk", "the-guardian-uk", "the-telegraph"};
        private const string apiKey = "&apiKey=daf0b98b42d34e6698bc1008f04e4849";


        public static async Task Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var mongoCollection = mongoDatabase.GetCollection<Article>("articles");
            mongoCollection.DeleteMany(new BsonDocument());

            var couchbaseBucket = couchbaseCluster.OpenBucket("articles");
            couchbaseBucket.CreateManager().Flush();


            var httpClient = new HttpClient();
            for (int i = 0; i < sources.Length; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    var response = await httpClient.GetAsync(apiPath + "&sources=" + sources[i] + apiKey + "&page=" + j);
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponseString = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponseString);

                        foreach (var a in apiResponse.Articles)
                        {
                            var article = new Article
                            {
                                Id = Guid.NewGuid().ToString(),
                                SourceName = a.Source.Name,
                                Author = a.Author,
                                Title = a.Title,
                                Description = a.Description,
                                Url = a.Url,
                                UrlToImage = a.UrlToImage,
                                PublishedAt = a.PublishedAt,
                                Content = a.Content
                            };

                            mongoCollection.InsertOne(article);
                            couchbaseBucket.Insert(article.Id, article);
                        }
                    }
                    else
                        Console.WriteLine("Inner connection with API failed");
                }
            }
        }
    }
}
