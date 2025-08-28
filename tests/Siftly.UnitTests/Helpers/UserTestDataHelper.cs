namespace Siftly.UnitTests.Helpers
{
	public static class UserTestDataHelper
	{		
		public static IQueryable<User> GetUsersQueryable()
		{
			var users = new List<User>
			{
				new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com", DateOfBirth = new DateTime(1990, 1, 1), Address = "123 Main St, Springfield", PhoneNumber = "555-1234", LastLogin = DateTime.UtcNow.AddDays(-1) },
				new User { Id = 2, Name = "Bob Johnson", Email = "bob.johnson@example.com", DateOfBirth = new DateTime(1985, 5, 23), Address = "456 Elm St, Shelbyville", PhoneNumber = "555-5678", LastLogin = null },
				new User { Id = 3, Name = "Charlie Brown", Email = "charlie.brown@example.com", DateOfBirth = new DateTime(2000, 12, 12), Address = "", PhoneNumber = "", LastLogin = DateTime.UtcNow.AddDays(-30) },
				new User { Id = 4, Name = "Dana White", Email = "dana.white@example.com", DateOfBirth = DateTime.UtcNow.AddYears(1), Address = "789 Oak St, Capital City", PhoneNumber = "555-8765", LastLogin = DateTime.UtcNow },
				new User { Id = 5, Name = "Élodie O'Connor", Email = "elodie.o'connor@example.co.uk", DateOfBirth = new DateTime(1995, 7, 15), Address = "321 Maple Ave, Ogdenville", PhoneNumber = "+44 20 7946 0958", LastLogin = DateTime.UtcNow.AddHours(-5) },
				new User { Id = 6, Name = "Frank Oldman", Email = "frank.oldman@example.com", DateOfBirth = new DateTime(1940, 3, 10), Address = "654 Pine St, North Haverbrook", PhoneNumber = "555-0000", LastLogin = DateTime.UtcNow.AddYears(-1) },
				new User { Id = 7, Name = "Grace Smith", Email = "alice@example.com", DateOfBirth = new DateTime(1992, 8, 8), Address = "987 Cedar St, Springfield", PhoneNumber = "555-4321", LastLogin = DateTime.UtcNow.AddMinutes(-10) },
				new User { Id = 8, Name = "Hannah Montana The Third of Her Name", Email = "hannah.montana@example.com", DateOfBirth = new DateTime(1988, 11, 30), Address = new string('A', 200), PhoneNumber = "555-9999", LastLogin = null }
			};
			return users.AsQueryable();
		}
	}
}
