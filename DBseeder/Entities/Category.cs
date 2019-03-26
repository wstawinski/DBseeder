using Couchbase.Linq.Filters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBseeder.Entities
{
    [DocumentTypeFilter("Category")]
    class Category
    {
        [BsonId]
        [Key]
        public Guid Id { get; set; }

        [BsonIgnore]
        [JsonProperty("type")]
        public string Type { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("parentCategoryId")]
        [JsonProperty("parentCategoryId")]
        public Guid ParentCategoryId { get; set; }

        [BsonElement("categoryPath")]
        [JsonProperty("categoryPath")]
        public List<string> CategoryPath { get; set; }
    }
}
