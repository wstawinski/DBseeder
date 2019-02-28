﻿using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBseeder.Models
{
    class OrderStatusHistoryUnit
    {
        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [BsonElement("dateStart")]
        [JsonProperty("dateStart")]
        public DateTime DateStart { get; set; }

        [BsonElement("dateEnd")]
        [JsonProperty("dateEnd")]
        public DateTime DateEnd { get; set; }
    }
}