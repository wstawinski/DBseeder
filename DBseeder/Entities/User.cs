﻿using Couchbase.Linq.Filters;
using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBseeder.Entities
{
    [DocumentTypeFilter("User")]
    class User
    {
        [BsonId]
        [Key]
        public Guid Id { get; set; }

        [BsonIgnore]
        [JsonProperty("type")]
        public string Type { get; set; }

        [BsonElement("firstName")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [BsonElement("birthDate")]
        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [BsonElement("hashedPassword")]
        [JsonProperty("hashedPassword")]
        public string HashedPassword { get; set; }

        [BsonElement("phoneNumber")]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("addresses")]
        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }
    }
}
