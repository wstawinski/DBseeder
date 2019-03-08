using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DBseeder.Entities
{
    class Category
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("parentCategoryId")]
        [JsonProperty("parentCategoryId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ParentCategoryId { get; set; }

        [BsonElement("categoryPath")]
        [JsonProperty("categoryPath")]
        public List<string> CategoryPath { get; set; }
    }
}
