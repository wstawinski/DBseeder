using Couchbase.Linq;
using DBseeder.Entities;
using DBseeder.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBseeder.EntitySeeders
{
    static class OrdersSeeder
    {
        private const string chars = "abcdefghijklmnoprstuwxyz";
        private static readonly Random random = new Random();


        public static void Seed(IMongoDatabase mongoDatabase, BucketContext couchbaseBucket)
        {
            var mongoCollection = mongoDatabase.GetCollection<Order>("orders");
            mongoCollection.DeleteMany(new BsonDocument());

            //Products
            var mongoProductsCollection = mongoDatabase.GetCollection<Product>("products");
            var mongoProducts = mongoProductsCollection.AsQueryable().Select(p => new { p.Id, p.Name, p.ActualPrice }).ToList();

            //Users
            var mongoUsersCollection = mongoDatabase.GetCollection<User>("users");
            var mongoUsers = mongoUsersCollection.AsQueryable().Select(u => new { u.Id, u.Addresses }).ToList();

            //Statusses
            var mongoStatussesCollection = mongoDatabase.GetCollection<OrderStatus>("orderStatusses");
            var mongoStatusses = mongoStatussesCollection.AsQueryable().ToList();

            //PaymentMethods
            var mongoPaymentMethodsCollection = mongoDatabase.GetCollection<PaymentMethod>("paymentMethods");
            var mongoPaymentMethods = mongoPaymentMethodsCollection.AsQueryable().ToList();

            //Seeding
            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2020, 1, 1);
            var range = (endDate - startDate).Days;

            for (int i = 0; i < mongoUsers.Count; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        Type = "Order",
                        UserId = mongoUsers[i].Id,
                        ProductsList = new List<OrderItem>(),
                        DateOrdered = DateTime.SpecifyKind(startDate.AddDays(random.Next(range)).AddHours(random.Next(24)).AddMinutes(random.Next(60)), DateTimeKind.Utc),
                        Status = mongoStatusses[random.Next(mongoStatusses.Count)],
                        PaymentMethod = mongoPaymentMethods[random.Next(mongoPaymentMethods.Count)],
                        DeliveryAddress = mongoUsers[i].Addresses[random.Next(mongoUsers[i].Addresses.Count)],
                        Cost = (decimal)random.Next(1000, 1000000) / 100
                    };

                    for (int k = 0; k < 5; k++)
                    {
                        var product = mongoProducts[random.Next(mongoProducts.Count)];
                        var orderItem = new OrderItem
                        {
                            ProductId = product.Id,
                            Name = product.Name,
                            ActualPrice = product.ActualPrice,
                            Quantity = random.Next(1, 6)
                        };
                        order.ProductsList.Add(orderItem);
                    }

                    mongoCollection.InsertOne(order);
                    couchbaseBucket.Save(order);
                }
            }
        }
    }
}
