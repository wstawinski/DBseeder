using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Couchbase.Linq;
using DBseeder.EntitySeeders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace DBseeder
{
    class Program
    {
        static void Main(string[] args)
        {
            //MongoDB connection configuration
            var mongoCredential = MongoCredential.CreateCredential("admin", "Administrator", "Password");
            var mongoSettings = new MongoClientSettings
            {
                Credential = mongoCredential,
                Server = new MongoServerAddress("localhost")
            };
            var mongoClient = new MongoClient(mongoSettings);
            var mongoDatabase = mongoClient.GetDatabase("database");
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

            //Couchbase connection configuration
            var couchbaseCluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://localhost:8091")}
            });
            var authenticator = new PasswordAuthenticator("Administrator", "Password");
            couchbaseCluster.Authenticate(authenticator);
            var couchbaseBucket = couchbaseCluster.OpenBucket("database");
            var couchbaseBucketContext = new BucketContext(couchbaseBucket);

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
                        case 1: CategoriesSeeder.Seed(mongoDatabase, couchbaseBucketContext);
                            break;
                        case 2: OrdersSeeder.Seed(mongoDatabase, couchbaseBucketContext);
                            break;
                        case 3: OrderStatusesSeeder.Seed(mongoDatabase, couchbaseBucketContext);
                            break;
                        case 4: PaymentMethodsSeeder.Seed(mongoDatabase, couchbaseBucketContext);
                            break;
                        case 5: ProductsSeeder.Seed(mongoDatabase, couchbaseBucketContext);
                            break;
                        case 6: ReviewsSeeder.Seed(mongoDatabase, couchbaseBucketContext);
                            break;
                        case 7: UsersSeeder.Seed(mongoDatabase, couchbaseBucketContext);
                            break;
                        case 8: ArticlesSeeder.Seed(mongoDatabase, couchbaseBucketContext).Wait();
                            break;
                        case 9:
                            Test(mongoDatabase, couchbaseBucketContext);
                            break;
                        default:
                            break;
                    }
                }
            }

            Console.WriteLine("Seeding finished");
            Console.ReadKey();
        }

        private static void Test(IMongoDatabase mongoDatabase, BucketContext couchbaseBucket)
        {

        }
    }
}