using Couchbase.Linq.Filters;
using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBseeder.Entities
{
    [DocumentTypeFilter("Order")]
    class Order
    {
        [BsonId]
        [Key]
        public Guid Id { get; set; }

        [BsonIgnore]
        [JsonProperty("type")]
        public string Type { get; set; }

        [BsonElement("userId")]
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [BsonElement("productsList")]
        [JsonProperty("productsList")]
        public List<OrderItem> ProductsList { get; set; }

        [BsonElement("dateOrdered")]
        [JsonProperty("dateOrdered")]
        public DateTime DateOrdered { get; set; }

        [BsonElement("status")]
        [JsonProperty("status")]
        public OrderStatus Status { get; set; }

        [BsonElement("cost")]
        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        [BsonElement("paymentMethod")]
        [JsonProperty("paymentMethod")]
        public PaymentMethod PaymentMethod { get; set; }

        [BsonElement("deliveryAddress")]
        [JsonProperty("deliveryAddress")]
        public Address DeliveryAddress { get; set; }
    }
}
