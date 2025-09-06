using Siftly.UnitTests.Model;

namespace Siftly.UnitTests.Helpers
{
    public static class UserTestDataHelper
    {
        public static IReadOnlyList<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Alice Smith",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    LastLogin = DateTime.UtcNow.AddDays(-1),
                    Verified = false,
                    HasLogged = false,
                    Address = new Address
                    {
                        Street = "123 Main St",
                        City = "Springfield"
                    },
                },
                new User
                {
                    Id = 2,
                    Name = "Bob Johnson",
                    DateOfBirth = new DateTime(1985, 5, 23),
                    LastLogin = null,
                    Verified = true,
                    HasLogged = true
                },
                new User
                {
                    Id = 3,
                    Name = "Charlie Brown",
                    DateOfBirth = new DateTime(2000, 12, 12),
                    LastLogin = DateTime.UtcNow.AddDays(-30),
                    Verified = true,
                    HasLogged = false
                },
                new User
                {
                    Id = 4,
                    Name = "Dana White",
                    DateOfBirth = DateTime.UtcNow.AddYears(1),
                    LastLogin = DateTime.UtcNow,
                    SubscriptionId = 3125674,
                    Verified = true,
                    HasLogged = false
                },
                new User
                {
                    Id = 5,
                    Name = "Élodie White",
                    DateOfBirth = new DateTime(1995, 7, 15),
                    LastLogin = DateTime.UtcNow.AddHours(-5),
                    SubscriptionId = 3125674,
                    Verified = true,
                    HasLogged = true,
                    Address = new Address
                    {
                        Street = "456 Elm St",
                        City = "Shelbyville"
                    },
                },
                new User
                {
                    Id = 6,
                    Name = "Frank Oldman",
                    DateOfBirth = new DateTime(1940, 3, 10),
                    LastLogin = DateTime.UtcNow.AddYears(-1),
                    SubscriptionId = 111555,
                    Address = new Address
                    {
                        Street = "456 Elm St",
                        City = "Shelbyville"
                    },
                },
                new User
                {
                    Id = 7,
                    Name = "Grace Smith",
                    DateOfBirth = new DateTime(1992, 8, 8),
                    LastLogin = DateTime.UtcNow.AddMinutes(-10),
                    Address = new Address
                    {
                        Street = string.Empty,
                        City = "Ogdenville"
                    },
                },
                new User
                {
                    Id = 8,
                    Name = "Hannah Montana The Third of Her Name",
                    DateOfBirth = new DateTime(1988, 11, 30),
                    LastLogin = null,
                    Verified = false,
                    HasLogged = false,
                    Address = new Address
                    {
                        Street = "   ",
                        City = "North Haverbrook"
                    },
                },
                new User
                {
                    Id = 9,
                    Name = "John Wick",
                    DateOfBirth = new DateTime(2005, 1, 11),
                    LastLogin = null,
                    Verified = true,
                    HasLogged = false,
                    Address = new Address
                    {
                        Street = "789 Oak St",
                        City = null
                    },
                },
                new User
                {
                    Id = 10,
                    Name = string.Empty,
                    DateOfBirth = new DateTime(2005, 1, 11),
                    LastLogin = new DateTime(1992, 8, 8),
                    Verified = false,
                    HasLogged = true,
                },
                new User
                {
                    Id = 11,
                    Name = "Alice Smith",
                    DateOfBirth = new DateTime(1998, 3, 19),
                    LastLogin = DateTime.UtcNow.AddDays(-11),
                    Verified = true,
                    HasLogged = true
                },
                new User
                {
                    Id = 12,
                    Name = null,
                    DateOfBirth = new DateTime(1957, 12, 11),
                    LastLogin = null,
                    Verified = true,
                    Address = new Address
                    {
                        Street = null,
                        City = null
                    },
                },
            };

            return users;
        }
    }
}