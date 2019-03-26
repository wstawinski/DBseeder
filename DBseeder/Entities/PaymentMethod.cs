using Couchbase.Linq.Filters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DBseeder.Entities
{
    [DocumentTypeFilter("PaymentMethod")]
    class PaymentMethod
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

        [BsonElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
