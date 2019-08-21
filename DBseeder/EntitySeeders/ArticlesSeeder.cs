using Couchbase.Linq;
using DBseeder.Entities;
using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DBseeder.EntitySeeders
{
    static class ArticlesSeeder
    {
        private const string apiPath = "https://newsapi.org/v2/everything?";
        private static readonly string[] sources = {  "abc-news", "bbc-news", "business-insider", "cnn", "daily-mail",
            "financial-times", "fox-news", "google-news", "the-new-york-times", "the-telegraph"};
        private const string apiKey = "f8dac3d617d345d7ab63fdd9a34ff5c9";


        public static async Task Seed(IMongoDatabase mongoDatabase, BucketContext couchbaseBucket)
        {
            var mongoCollection = mongoDatabase.GetCollection<Article>("articles");
            mongoCollection.DeleteMany(new BsonDocument());

            var today = DateTime.Now.Date;
            var httpClient = new HttpClient();
            for (int i = 0; i < sources.Length; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    var day = today.AddDays(-1 * j).ToString("yyyy-MM-dd");

                    var response = await httpClient.GetAsync(apiPath + "sources=" + sources[i] + "&from=" + day + "&to=" + day + "&pageSize=100" + "&apiKey=" + apiKey);
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponseString = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponseString);

                        foreach (var a in apiResponse.Articles)
                        {
                            var article = new Article
                            {
                                Type = "Article",
                                SourceName = a.Source.Name,
                                Author = a.Author,
                                Title = a.Title,
                                Description = a.Description,
                                Url = a.Url,
                                UrlToImage = a.UrlToImage,
                                PublishedAt = DateTime.Parse(a.PublishedAt).ToUniversalTime(),
                                Content = a.Content
                            };

                            for (int k = 0; k < 1; k++)
                            {
                                article.Id = Guid.NewGuid();
                                mongoCollection.InsertOne(article);
                                couchbaseBucket.Save(article);
                            }
                        }
                    }
                    else
                        Console.WriteLine("Inner connection with API failed");
                }
            }
        }
    }
}
