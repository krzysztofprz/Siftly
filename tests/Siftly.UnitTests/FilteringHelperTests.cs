using Siftly.Helpers.Queryable;
using Siftly.UnitTests.Helpers;
using Siftly.UnitTests.Model;

namespace Siftly.UnitTests
{
    [TestFixture]
    public class FilteringHelperTests
    {
        private IQueryable<User> _users;

        [SetUp]
        public void Setup()
        {
            _users = UserTestDataHelper.GetUsers().AsQueryable();
        }

        [TestCase(nameof(User.Id), 1, 1)]
        [TestCase(nameof(User.Id), 2, 1)]
        [TestCase(nameof(User.Id), 11, 1)]
        [TestCase(nameof(User.Id), 12, 1)]
        [TestCase(nameof(User.Id), 0, 0)]
        [TestCase(nameof(User.Id), 55, 0)]
        public void FilterT_IntPropertyType_IdProperty_ReturnsFiltered(
            string filteringProperty,
            object filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.SubscriptionId), 111555, 1)]
        [TestCase(nameof(User.SubscriptionId), 3125674, 2)]
        public void FilterT_NullableIntPropertyType_SubscriptionIdProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            object filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.SubscriptionId), 9)]
        public void FilterT_NullableIntPropertyType_SubscriptionIdProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object subscriptionId = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, subscriptionId).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), "Alice Smith", 2)]
        [TestCase(nameof(User.Name), " ", 0)]
        [TestCase(nameof(User.Name), "    ", 0)]
        public void FilterT_StringPropertyType_NameProperty_NotNullOrEmpty_ReturnsFiltered(
            string filteringProperty,
            object filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), 1)]
        public void FilterT_StringPropertyType_NameProperty_Empty_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object filteringValue = "";

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), 1)]
        public void FilterT_StringPropertyType_NameProperty_StringEmpty_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object filteringValue = string.Empty;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), 1)]
        public void FilterT_StringPropertyType_NameProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object filteringValue = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.DateOfBirth), "2005-1-11", 2)]
        [TestCase(nameof(User.DateOfBirth), "1998-3-19", 1)]
        [TestCase(nameof(User.DateOfBirth), "1940-03-10", 1)]
        [TestCase(nameof(User.DateOfBirth), "1990-1-01", 1)]
        [TestCase(nameof(User.DateOfBirth), "2220-01-01", 0)]
        public void FilterT_DateTimePropertyType_DateOfBirthProperty_ReturnsFiltered(
            string filteringProperty,
            string filteringValue,
            int expectedCount)
        {
            // Arrange
            object dateOfBirth = DateTime.Parse(filteringValue);

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, dateOfBirth).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.LastLogin), "1992-8-8", 1)]
        [TestCase(nameof(User.LastLogin), "2101-1-1", 0)]
        public void FilterT_NullableDateTimePropertyType_LastLoginProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            string filteringValue,
            int expectedCount)
        {
            // Arrange
            object lastLogin = DateTime.Parse(filteringValue);

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, lastLogin).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.LastLogin), 4)]
        public void FilterT_NullableDateTimePropertyType_LastLoginProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object lastLogin = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, lastLogin).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Verified), true, 7)]
        [TestCase(nameof(User.Verified), false, 5)]
        public void FilterT_BoolPropertyType_VerifiedProperty_ReturnsFiltered(
            string filteringProperty,
            bool filteringValue,
            int expectedCount)
        {
            // Arrange
            object verified = filteringValue;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, verified).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.HasLogged), true, 4)]
        [TestCase(nameof(User.HasLogged), false, 5)]
        public void FilterT_NullableBoolPropertyType_HasLoggedProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            bool filteringValue,
            int expectedCount)
        {
            // Arrange
            object hasLogged = filteringValue;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, hasLogged).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.HasLogged), 3)]
        public void FilterT_NullableBoolPropertyType_HasLoggedProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object hasLogged = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, hasLogged).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "Springfield", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "Shelbyville", 2)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "", 0)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", " ", 0)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "   ", 0)]
        public void FilterT_NestedPropertyType_AddressCityProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            object filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", 7)]
        public void FilterT_NestedPropertyType_AddressCityProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object addressCity = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, addressCity).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "789 Oak St", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "456 Elm St", 2)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "   ", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", " ", 0)]
        public void FilterT_NestedPropertyType_AddressStreetProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            object filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", 1)]
        public void FilterT_NestedPropertyType_AddressStreetProperty_StringEmpty_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object street = string.Empty;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, street).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", 6)]
        public void FilterT_NestedPropertyType_AddressStreetProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            object addressStreet = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, addressStreet).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Id), 1, 1)]
        [TestCase(nameof(User.Id), 2, 1)]
        [TestCase(nameof(User.Id), 11, 1)]
        [TestCase(nameof(User.Id), 12, 1)]
        [TestCase(nameof(User.Id), 0, 0)]
        [TestCase(nameof(User.Id), 55, 0)]
        public void FilterTS_IntPropertyType_IdProperty_ReturnsFiltered(
            string filteringProperty,
            int filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.SubscriptionId), 111555, 1)]
        [TestCase(nameof(User.SubscriptionId), 3125674, 2)]
        public void FilterTS_NullableIntPropertyType_SubscriptionIdProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            int? filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.SubscriptionId), 9)]
        public void FilterTS_NullableIntPropertyType_SubscriptionIdProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            int? subscriptionId = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, subscriptionId).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), "Alice Smith", 2)]
        [TestCase(nameof(User.Name), " ", 0)]
        [TestCase(nameof(User.Name), "    ", 0)]
        public void FilterTS_StringPropertyType_NameProperty_NotNullOrEmpty_ReturnsFiltered(
            string filteringProperty,
            string filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), 1)]
        public void FilterTS_StringPropertyType_NameProperty_Empty_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            string filteringValue = "";

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), 1)]
        public void FilterTS_StringPropertyType_NameProperty_StringEmpty_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            string filteringValue = string.Empty;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Name), 1)]
        public void FilterTS_StringPropertyType_NameProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            string filteringValue = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.DateOfBirth), "2005-1-11", 2)]
        [TestCase(nameof(User.DateOfBirth), "1998-3-19", 1)]
        [TestCase(nameof(User.DateOfBirth), "1940-03-10", 1)]
        [TestCase(nameof(User.DateOfBirth), "1990-1-01", 1)]
        [TestCase(nameof(User.DateOfBirth), "2220-01-01", 0)]
        public void FilterTS_DateTimePropertyType_DateOfBirthProperty_ReturnsFiltered(
            string filteringProperty,
            string filteringValue,
            int expectedCount)
        {
            // Arrange
            DateTime dateOfBirth = DateTime.Parse(filteringValue);

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, dateOfBirth).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.LastLogin), "1992-8-8", 1)]
        [TestCase(nameof(User.LastLogin), "2101-1-1", 0)]
        public void FilterTS_NullableDateTimePropertyType_LastLoginProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            string filteringValue,
            int expectedCount)
        {
            // Arrange
            DateTime? lastLogin = DateTime.Parse(filteringValue);

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, lastLogin).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.LastLogin), 4)]
        public void FilterTS_NullableDateTimePropertyType_LastLoginProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            DateTime? lastLogin = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, lastLogin).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.Verified), true, 7)]
        [TestCase(nameof(User.Verified), false, 5)]
        public void FilterTS_BoolPropertyType_VerifiedProperty_ReturnsFiltered(
            string filteringProperty,
            bool filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.HasLogged), true, 4)]
        [TestCase(nameof(User.HasLogged), false, 5)]
        public void FilterTS_NullableBoolPropertyType_HasLoggedProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            bool filteringValue,
            int expectedCount)
        {
            // Arrange
            bool? hasLogged = filteringValue;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, hasLogged).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(nameof(User.HasLogged), 3)]
        public void FilterTS_NullableBoolPropertyType_HasLoggedProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            bool? hasLogged = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, hasLogged).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "Springfield", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "Shelbyville", 2)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "", 0)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", " ", 0)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", "   ", 0)]
        public void FilterTS_NestedPropertyType_AddressCityProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            string filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", 7)]
        public void FilterTS_NestedPropertyType_AddressCityProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            string nullCity = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, nullCity).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "789 Oak St", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "456 Elm St", 2)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "   ", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", "", 1)]
        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", " ", 0)]
        public void FilterTS_NestedPropertyType_AddressStreetProperty_NotNull_ReturnsFiltered(
            string filteringProperty,
            string filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", 1)]
        public void FilterTS_NestedPropertyType_AddressStreetProperty_StringEmpty_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            var emptyStreet = string.Empty;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, emptyStreet).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", 6)]
        public void FilterTS_NestedPropertyType_AddressStreetProperty_Null_ReturnsFiltered(
            string filteringProperty,
            int expectedCount)
        {
            // Arrange
            string nullStreet = null;

            // Act
            var result = FilteringHelper.Filter(_users, filteringProperty, nullStreet).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(11, 1)]
        [TestCase(12, 1)]
        [TestCase(0, 0)]
        [TestCase(55, 0)]
        public void FilterTS_IntPropertyType_IdProperty_ReturnsFiltered(
            int filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, user => user.Id, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(111555, 1)]
        [TestCase(3125674, 2)]
        public void FilterTS_NullableIntPropertyType_SubscriptionIdProperty_NotNull_ReturnsFiltered(
            int? filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, user => user.SubscriptionId, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(9)]
        public void FilterTS_NullableIntPropertyType_SubscriptionIdProperty_Null_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            int? subscriptionId = null;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.SubscriptionId, subscriptionId).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase("Alice Smith", 2)]
        [TestCase(" ", 0)]
        [TestCase("    ", 0)]
        public void FilterTS_StringPropertyType_NameProperty_NotNullOrEmpty_ReturnsFiltered(
            string filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, user => user.Name, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(1)]
        public void FilterTS_StringPropertyType_NameProperty_Empty_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            string filteringValue = "";

            // Act
            var result = FilteringHelper.Filter(_users, user => user.Name, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(1)]
        public void FilterTS_StringPropertyType_NameProperty_StringEmpty_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            string filteringValue = string.Empty;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.Name, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(1)]
        public void FilterTS_StringPropertyType_NameProperty_Null_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            string filteringValue = null;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.Name, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase("2005-1-11", 2)]
        [TestCase("1998-3-19", 1)]
        [TestCase("1940-03-10", 1)]
        [TestCase("1990-1-01", 1)]
        [TestCase("2220-01-01", 0)]
        public void FilterTS_DateTimePropertyType_DateOfBirthProperty_ReturnsFiltered(
            string filteringValue,
            int expectedCount)
        {
            // Arrange
            DateTime dateOfBirth = DateTime.Parse(filteringValue);

            // Act
            var result = FilteringHelper.Filter(_users, user => user.DateOfBirth, dateOfBirth).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase("1992-8-8", 1)]
        [TestCase("2101-1-1", 0)]
        public void FilterTS_NullableDateTimePropertyType_LastLoginProperty_NotNull_ReturnsFiltered(
            string filteringValue,
            int expectedCount)
        {
            // Arrange
            DateTime? lastLogin = DateTime.Parse(filteringValue);

            // Act
            var result = FilteringHelper.Filter(_users, user => user.LastLogin, lastLogin).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(4)]
        public void FilterTS_NullableDateTimePropertyType_LastLoginProperty_Null_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            DateTime? lastLogin = null;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.LastLogin, lastLogin).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(true, 7)]
        [TestCase(false, 5)]
        public void FilterTS_BoolPropertyType_VerifiedProperty_ReturnsFiltered(
            bool filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, user => user.Verified, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(true, 4)]
        [TestCase(false, 5)]
        public void FilterTS_NullableBoolPropertyType_HasLoggedProperty_NotNull_ReturnsFiltered(
            bool filteringValue,
            int expectedCount)
        {
            // Arrange
            bool? hasLogged = filteringValue;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.HasLogged, hasLogged).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(3)]
        public void FilterTS_NullableBoolPropertyType_HasLoggedProperty_Null_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            bool? hasLogged = null;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.HasLogged, hasLogged).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase("Springfield", 1)]
        [TestCase("Shelbyville", 2)]
        [TestCase("", 0)]
        [TestCase(" ", 0)]
        [TestCase("   ", 0)]
        public void FilterTS_NestedPropertyType_AddressCityProperty_NotNull_ReturnsFiltered(
            string filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, user => user.Address.City, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(7)]
        public void FilterTS_NestedPropertyType_AddressCityProperty_Null_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            string nullCity = null;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.Address.City, nullCity).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase("789 Oak St", 1)]
        [TestCase("456 Elm St", 2)]
        [TestCase("   ", 1)]
        [TestCase("", 1)]
        [TestCase(" ", 0)]
        public void FilterTS_NestedPropertyType_AddressStreetProperty_NotNull_ReturnsFiltered(
            string filteringValue,
            int expectedCount)
        {
            // Act
            var result = FilteringHelper.Filter(_users, user => user.Address.Street, filteringValue).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(1)]
        public void FilterTS_NestedPropertyType_AddressStreetProperty_StringEmpty_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            var emptyStreet = string.Empty;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.Address.Street, emptyStreet).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }

        [TestCase(6)]
        public void FilterTS_NestedPropertyType_AddressStreetProperty_Null_ReturnsFiltered(
            int expectedCount)
        {
            // Arrange
            string nullStreet = null;

            // Act
            var result = FilteringHelper.Filter(_users, user => user.Address.Street, nullStreet).ToList();

            // Assert
            Assert.That(actual: result.Count, Is.EqualTo(expectedCount));
        }
    }
}