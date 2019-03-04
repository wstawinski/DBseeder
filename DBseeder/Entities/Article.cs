using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBseeder.Entities
{
    class Article
    {
        public string Id { get; set; }

        [BsonElement("sourceName")]
        [JsonProperty("sourceName")]
        public string SourceName { get; set; }

        [BsonElement("author")]
        [JsonProperty("author")]
        public string Author { get; set; }

        [BsonElement("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [BsonElement("url")]
        [JsonProperty("url")]
        public string Url { get; set; }

        [BsonElement("urlToImage")]
        [JsonProperty("urlToImage")]
        public string UrlToImage { get; set; }

        [BsonElement("publishedAt")]
        [JsonProperty("publishedAt")]
        public string PublishedAt { get; set; }

        [BsonElement("content")]
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
