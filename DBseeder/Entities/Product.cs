using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DBseeder.Entities
{
    class Product
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("categoryId")]
        [JsonProperty("categoryId")]
        [BsonRepresentation(BsonType.String)]
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

        [BsonElement("priceHistory")]
        [JsonProperty("priceHistory")]
        public List<ProductPrice> PriceHistory { get; set; }

        [BsonElement("accessories")]
        [JsonProperty("accessories")]
        public List<ProductAccessory> Accessories { get; set; }

        [BsonElement("averageRating")]
        [JsonProperty("averageRating")]
        public int AverageRating { get; set; }

        [BsonElement("reviewsCount")]
        [JsonProperty("reviewsCount")]
        public int ReviewsCount { get; set; }
    }
}
