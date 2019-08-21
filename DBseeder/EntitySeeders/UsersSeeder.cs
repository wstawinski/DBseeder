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
    class UsersSeeder
    {
        private static readonly string[] firstNames =
        {
            "Adam", "Adrian", "Alan", "Alexander", "Andrew", "Anthony", "Austin", "Benjamin", "Blake", "Boris",
            "Brandon", "Brian", "Cameron", "Carl", "Charles", "Christian", "Christopher", "Colin", "Connor", "Dan",
            "David", "Dominic", "Dylan", "Edward", "Eric", "Ella", "Emily", "Emma", "Faith", "Felicity",
            "Fiona", "Gabrielle", "Grace", "Hannah", "Heather", "Irene", "Jan", "Jane", "Jasmine", "Jennifer",
            "Jessica", "Joan", "Joanne", "Julia", "Karen", "Katherine", "Kimberly", "Kylie", "Lauren", "Leah"
        };
        private static readonly string[] lastNames =
        {
            "Abraham", "Allan", "Alsop", "Anderson", "Arnold", "Avery", "Bailey", "Baker", "Ball", "Bell",
            "Berry", "Black", "Blake", "Bond", "Bower", "Brown", "Buckland", "Burgess", "Butler", "Cameron",
            "Campbell", "Carr", "Chapman", "Churchill", "Clark", "Clarkson", "Coleman", "Cornish", "Davidson", "Davies",
            "Dickens", "Dowd", "Duncan", "Dyer", "Edmunds", "Ellison", "Ferguson", "Fisher", "Forsyth", "Fraser",
            "Gibson", "Gill", "Glover", "Graham", "Grant", "Gray", "Greene", "Hamilton", "Hardacre", "Harris"
        };
        private static readonly string[] streets =
        {
            "Locust Lane", "Highland Avenue", "6th Avenue", "Hilltop Road", "Poplar Street",
            "11th Street", "9th Street", "Brook Lane", "Valley Drive", "Howard Street",
            "Strawberry Lane", "Front Street North", "Lexington Drive", "Lake Street", "Ridge Avenue",
            "Route 41", "8th Street South", "Hickory Lane", "Creek Road", "Hawthorne Avenue",
            "2nd Avenue", "Spruce Avenue", "Spruce Street", "Berkshire Drive", "Shady Lane"
        };
        private static readonly string[] cities =
        {
            "Nueso", "Ontamona", "Vedad", "Bara", "Oloquina",
            "Cancanos", "Mixtitlan", "San Antiapa", "Camada", "San Jejito",
            "Posolzan", "Matilali", "Mutova", "Sordasi", "Camaso",
            "Seizon", "Cartalupe", "San Vilen", "Jusonate", "Lolotmoros",
            "Oraban", "Asunquimula", "La Aliaranza", "El Sucaran", "Prisio"
        };

        private const string specials = "abcdefghijklmnoprstuwxyz1234567890!@#$%^&*()";
        private const string chars = "abcdefghijklmnoprstuwxyz";
        private const string digits = "1234567890";
        private static readonly Random random = new Random();


        public static void Seed(IMongoDatabase mongoDatabase, BucketContext couchbaseBucket)
        {
            var mongoCollection = mongoDatabase.GetCollection<User>("users");
            mongoCollection.DeleteMany(new BsonDocument());

            var startDate = new DateTime(1950, 1, 1);
            var endDate = new DateTime(2000, 1, 1);
            var range = (endDate - startDate).Days;

            for (int i = 0; i < 1000; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Type = "User",
                    BirthDate = DateTime.SpecifyKind(startDate.AddDays(random.Next(range)), DateTimeKind.Utc),
                    HashedPassword = new string(Enumerable.Repeat(specials, 10).Select(s => s[random.Next(s.Length)]).ToArray()),
                    PhoneNumber = new string(Enumerable.Repeat(digits, 9).Select(s => s[random.Next(s.Length)]).ToArray()),
                    Addresses = new List<Address>()
                };

                if (i < 1000)
                {
                    user.FirstName = firstNames[random.Next(firstNames.Length)];
                    user.LastName = lastNames[random.Next(lastNames.Length)];

                    var username = user.FirstName + "_" + user.LastName;
                    var exists = true;
                    var counter = 1;
                    while (exists)
                    {
                        var userFromDb = mongoCollection.AsQueryable().Where(u => u.Username == username).SingleOrDefault();
                        if (userFromDb != null)
                        {
                            username += counter;
                            counter++;
                        }
                        else
                            exists = false;
                    }

                    user.Username = username;
                    user.Email = username + "@mail.com";
                }
                else
                {
                    user.FirstName = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                    user.LastName = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                    user.Username = user.FirstName + "_" + user.LastName;
                    user.Email = user.Username + "@mail.com";
                }

                var addressesCount = random.Next(1, 4);
                for (int j = 0; j < addressesCount; j++)
                {
                    var address = new Address
                    {
                        Street = streets[random.Next(streets.Length)],
                        HouseNumber = random.Next(1, 101),
                        FlatNumber = random.Next(0, 21),
                        PostalCode = random.Next(90, 100) + "-" + random.Next(1000).ToString("000"),
                        City = cities[random.Next(cities.Length)]
                    };

                    user.Addresses.Add(address);
                }

                mongoCollection.InsertOne(user);
                couchbaseBucket.Save(user);
            }
        }
    }
}
