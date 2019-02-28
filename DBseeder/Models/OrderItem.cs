using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBseeder.Models
{
    class OrderItem
    {
        [BsonElement("productId")]
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("actualPrice")]
        [JsonProperty("actualPrice")]
        public double ActualPrice { get; set; }

        [BsonElement("quantity")]
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
