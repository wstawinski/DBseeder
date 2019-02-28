using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBseeder.Models
{
    class Address
    {
        [BsonElement("street")]
        [JsonProperty("street")]
        public string Street { get; set; }

        [BsonElement("houseNumber")]
        [JsonProperty("houseNumber")]
        public int HouseNumber { get; set; }

        [BsonElement("flatNumber")]
        [JsonProperty("flatNumber")]
        public int? FlatNumber { get; set; }

        [BsonElement("postalCode")]
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [BsonElement("city")]
        [JsonProperty("city")]
        public string City { get; set; }
    }
}
