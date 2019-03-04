using Couchbase;
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


        public static void Seed(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {
            var mongoCollection = mongoDatabase.GetCollection<Order>("orders");
            mongoCollection.DeleteMany(new BsonDocument());

            var couchbaseBucket = couchbaseCluster.OpenBucket("orders");
            couchbaseBucket.CreateManager().Flush();

            //Products
            var mongoProductsCollection = mongoDatabase.GetCollection<Product>("products");
            var mongoProducts = mongoProductsCollection.AsQueryable().ToList();

            //Users
            var mongoUsersCollection = mongoDatabase.GetCollection<User>("users");
            var mongoUsers = mongoUsersCollection.AsQueryable().ToList();

            //Statusses
            var mongoStatussesCollection = mongoDatabase.GetCollection<OrderStatus>("orderStatusses");
            var mongoStatusses = mongoStatussesCollection.AsQueryable().ToList();

            //PaymentMethods
            var mongoPaymentMethodsCollection = mongoDatabase.GetCollection<PaymentMethod>("paymentMethods");
            var mongoPaymentMethods = mongoPaymentMethodsCollection.AsQueryable().ToList();

            //Seeding
            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2019, 1, 1);
            var range = (endDate - startDate).Days;

            for (int i = 0; i < mongoUsers.Count; i++)
            {
                var ordersCount = random.Next(11);
                for (int j = 0; j < ordersCount; j++)
                {
                    var order = new Order
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = mongoUsers[i].Id,
                        ProductsList = new List<OrderItem>(),
                        DateOrdered = startDate.AddDays(random.Next(range)).AddHours(random.Next(24)).AddMinutes(random.Next(60)).ToString(),
                        Status = mongoStatusses[random.Next(mongoStatusses.Count)],
                        StatusHistory = new List<OrderStatusHistoryUnit>(),
                        PaymentMethod = mongoPaymentMethods[random.Next(mongoPaymentMethods.Count)],
                        DeliveryAddress = mongoUsers[i].Addresses[random.Next(mongoUsers[i].Addresses.Count)]
                    };

                    var orderItemsCount = random.Next(1, 6);
                    for (int k = 0; k < orderItemsCount; k++)
                    {
                        var product = mongoProducts[random.Next(mongoProducts.Count)];
                        var orderItem = new OrderItem
                        {
                            ProductId = product.Id,
                            Name = product.Name,
                            ActualPrice = product.ActualPrice,
                            Quantity = random.Next(6)
                        };
                        order.ProductsList.Add(orderItem);
                    }

                    var orderStatusHistoryUnits = random.Next(7);
                    for (int k = 0; k < orderStatusHistoryUnits; k++)
                    {
                        var statusHistoryUnit = new OrderStatusHistoryUnit
                        {
                            Name = mongoStatusses[k].Name,
                            DateStart = startDate.AddDays(random.Next(range)).AddHours(random.Next(24)).AddMinutes(random.Next(60)).ToString(),
                            DateEnd = startDate.AddDays(random.Next(range)).AddHours(random.Next(24)).AddMinutes(random.Next(60)).ToString()
                        };
                        order.StatusHistory.Add(statusHistoryUnit);
                    }

                    var cost = 0.0;
                    foreach (var orderItem in order.ProductsList)
                    {
                        cost += orderItem.ActualPrice * orderItem.Quantity;
                    }
                    order.Cost = cost;

                    mongoCollection.InsertOne(order);
                    couchbaseBucket.Insert(order.Id, order);
                }
            }
        }
    }
}
