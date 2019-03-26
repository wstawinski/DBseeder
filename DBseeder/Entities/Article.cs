using Couchbase.Linq.Filters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DBseeder.Entities
{
    [DocumentTypeFilter("Article")]
    class Article
    {
        [BsonId]
        [Key]
        public Guid Id { get; set; }

        [BsonIgnore]
        [JsonProperty("type")]
        public string Type { get; set; }

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
        public DateTime PublishedAt { get; set; }

        [BsonElement("content")]
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
