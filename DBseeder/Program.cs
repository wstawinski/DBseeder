using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using DBseeder.Entities;
using DBseeder.EntitySeeders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DBseeder
{
    class Program
    {
        static void Main(string[] args)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("database");
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

            var couchbaseCluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://localhost:8091")}
            });
            var authenticator = new PasswordAuthenticator("Administrator", "Password");
            couchbaseCluster.Authenticate(authenticator);

            int choice = 10;
            while (choice != 0)
            {
                Console.WriteLine("Choose entity to seed:");
                Console.WriteLine("1 - Categories");
                Console.WriteLine("2 - Orders");
                Console.WriteLine("3 - OrderStatuses");
                Console.WriteLine("4 - PaymentMethods");
                Console.WriteLine("5 - Products");
                Console.WriteLine("6 - Reviews");
                Console.WriteLine("7 - Users");
                Console.WriteLine("8 - Articles");
                Console.WriteLine("9 - Test");
                Console.WriteLine("0 - Exit");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1: CategoriesSeeder.Seed(mongoDatabase, couchbaseCluster);
                            break;
                        case 2: OrdersSeeder.Seed(mongoDatabase, couchbaseCluster);
                            break;
                        case 3: OrderStatusesSeeder.Seed(mongoDatabase, couchbaseCluster);
                            break;
                        case 4: PaymentMethodsSeeder.Seed(mongoDatabase, couchbaseCluster);
                            break;
                        case 5: ProductsSeeder.Seed(mongoDatabase, couchbaseCluster);
                            break;
                        case 6: ReviewsSeeder.Seed(mongoDatabase, couchbaseCluster);
                            break;
                        case 7: UsersSeeder.Seed(mongoDatabase, couchbaseCluster);
                            break;
                        case 8: ArticlesSeeder.Seed(mongoDatabase, couchbaseCluster).Wait();
                            break;
                        case 9:
                            Test(mongoDatabase, couchbaseCluster);
                            break;
                        default:
                            break;
                    }
                }
            }

            Console.WriteLine("Seeding finished");
            Console.ReadKey();
        }

        private static void Test(IMongoDatabase mongoDatabase, Cluster couchbaseCluster)
        {

        }
    }
}
