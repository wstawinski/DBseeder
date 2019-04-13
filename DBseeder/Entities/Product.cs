using Couchbase.Linq.Filters;
using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBseeder.Entities
{
    [DocumentTypeFilter("Product")]
    class Product
    {
        [BsonId]
        [Key]
        public Guid Id { get; set; }

        [BsonIgnore]
        [JsonProperty("type")]
        public string Type { get; set; }

        [BsonElement("categoryId")]
        [JsonProperty("categoryId")]
        public Guid CategoryId { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [BsonElement("details")]
        [JsonProperty("details")]
        public List<ProductDetail> Details { get; set; }

        [BsonElement("quantity")]
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [BsonElement("actualPrice")]
        [JsonProperty("actualPrice")]
        public decimal ActualPrice { get; set; }

        [BsonElement("dateAdded")]
        [JsonProperty("dateAdded")]
        public DateTime DateAdded { get; set; }

        [BsonElement("averageRating")]
        [JsonProperty("averageRating")]
        public int AverageRating { get; set; }

        [BsonElement("reviewsCount")]
        [JsonProperty("reviewsCount")]
        public int ReviewsCount { get; set; }
    }
}
