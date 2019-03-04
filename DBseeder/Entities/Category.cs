using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBseeder.Entities
{
    class Category
    {
        public string Id { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("parentCategoryId")]
        [JsonProperty("parentCategoryId")]
        public string ParentCategoryId { get; set; }

        [BsonElement("categoryPath")]
        [JsonProperty("categoryPath")]
        public List<string> CategoryPath { get; set; }
    }
}
