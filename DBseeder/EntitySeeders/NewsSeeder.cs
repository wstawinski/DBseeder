using Couchbase;
using DBseeder.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DBseeder.EntitySeeders
{
    class NewsSeeder
    {
        private const string apiPath = "https://newsapi.org/v2/everything";
        private static readonly string[] sources = { "bbc-news", "daily-mail", "the-guardian-uk" };
        private const string apiKey = "daf0b98b42d34e6604e4849";


        public static async Task Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiPath + apiKey);

            if (response.IsSuccessStatusCode)
            {
                var apiResponseString = await response.Content.ReadAsStringAsync();
                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponseString);
                }
                catch (Exception)
                {
                    Console.WriteLine("Deserialization failed");
                }
            }
            else
                Console.WriteLine("Connection with API failed");
           
        }
    }
}
