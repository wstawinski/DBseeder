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
    class Review
    {
        public string Id { get; set; }

        [BsonElement("productId")]
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [BsonElement("userId")]
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        [BsonElement("rating")]
        [JsonProperty("rating")]
        public int Rating { get; set; }

        [BsonElement("text")]
        [JsonProperty("text")]
        public string Text { get; set; }

        [BsonElement("dateAdded")]
        [JsonProperty("dateAdded")]
        public string DateAdded { get; set; }

        [BsonElement("helpfulVotes")]
        [JsonProperty("helpfulVotes")]
        public int HelpfulVotes { get; set; }

        [BsonElement("unhelpfulVotes")]
        [JsonProperty("unhelpfulVotes")]
        public int UnhelpfulVotes { get; set; }
    }
}
