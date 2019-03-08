using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DBseeder.Entities
{
    class Order
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("userId")]
        [JsonProperty("userId")]
        [BsonRepresentation(BsonType.String)]
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

        [BsonElement("statusHistory")]
        [JsonProperty("statusHistory")]
        public List<OrderStatusHistoryUnit> StatusHistory { get; set; }

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
