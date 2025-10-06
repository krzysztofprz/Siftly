using Siftly.Helpers;
using Siftly.Helpers.Queryable;
using Siftly.Model;
using Siftly.UnitTests.Helpers;
using Siftly.UnitTests.Model;

namespace Siftly.UnitTests;

[TestFixture]
public class PaginationHelperTests
{
    private IQueryable<User> _users;

    [SetUp]
    public void Setup()
    {
        _users = UserTestDataHelper.GetUsers().AsQueryable();
    }

    [Test]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 0, 12, 12, 1)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 2, 12, 10, 3)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 0, 12, 12, 12)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 2, 12, 10, 10)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 5, 5, 5, 7)]
    public void OffsetT_OrderByIntPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount,
        int expectedId)
    {
        // Act
        var page = PaginationHelper.Offset(_users, sortingProperty, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Id, Is.EqualTo(expectedId));
    }

    [Test]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 12, 1)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 12, 1)]
    public void OffsetT_OrderByIntPropertyType_SkipAll_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int skip,
        int take)
    {
        // Act
        var page = PaginationHelper.Offset(_users, sortingProperty, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(0));
    }

    [Test]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 0, 15, 12)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 5, 15, 7)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 15, 10, 0)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 0, 15, 12)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 15, 10, 0)]
    public void OffsetT_OrderByIntPropertyType_TakeMore_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount)
    {
        // Act
        var page = PaginationHelper.Offset(_users, sortingProperty, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
    }

    [Test]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, 0, 12, 12, null)]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, 2, 12, 10, "Alice Smith")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, 0, 12, 12, "John Wick")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, 2, 12, 10, "Grace Smith")]
    public void OffsetT_OrderByStringPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Offset(_users, sortingProperty, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Name, Is.EqualTo(expectedName));
    }

    [Test]
    [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", SortingDirection.Ascending, 0, 12, 12, null)]
    [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", SortingDirection.Ascending, 7, 5, 5, "   ")]
    [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", SortingDirection.Descending, 0, 12, 12, "789 Oak St")]
    [TestCase($"{nameof(User.Address)}.{nameof(Address.Street)}", SortingDirection.Descending, 2, 5, 5, "456 Elm St")]
    public void OffsetT_OrderByNestedPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount,
        string expectedStreet)
    {
        // Act
        var page = PaginationHelper.Offset(_users, sortingProperty, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Address?.Street, Is.EqualTo(expectedStreet));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, 0, 12, 12, 1)]
    [TestCase(SortingDirection.Ascending, 2, 12, 10, 3)]
    [TestCase(SortingDirection.Descending, 0, 12, 12, 12)]
    [TestCase(SortingDirection.Descending, 2, 12, 10, 10)]
    [TestCase(SortingDirection.Descending, 5, 5, 5, 7)]
    public void OffsetTS_OrderByIntPropertyType_ReturnsPaginated(
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount,
        int expectedId)
    {
        // Act
        var page = PaginationHelper.Offset(_users, user => user.Id, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Id, Is.EqualTo(expectedId));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, 12, 1)]
    [TestCase(SortingDirection.Descending, 12, 1)]
    public void OffsetTS_OrderByIntPropertyType_SkipAll_ReturnsPaginated(
        SortingDirection sortingDirection,
        int skip,
        int take)
    {
        // Act
        var page = PaginationHelper.Offset(_users, user => user.Id, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(0));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, 0, 15, 12)]
    [TestCase(SortingDirection.Ascending, 5, 15, 7)]
    [TestCase(SortingDirection.Ascending, 15, 10, 0)]
    [TestCase(SortingDirection.Descending, 0, 15, 12)]
    [TestCase(SortingDirection.Descending, 15, 10, 0)]
    public void OffsetT_OrderByIntPropertyType_TakeMore_ReturnsPaginated(
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount)
    {
        // Act
        var page = PaginationHelper.Offset(_users, user => user.Id, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, 0, 12, 12, null)]
    [TestCase(SortingDirection.Ascending, 2, 12, 10, "Alice Smith")]
    [TestCase(SortingDirection.Descending, 0, 12, 12, "John Wick")]
    [TestCase(SortingDirection.Descending, 2, 12, 10, "Grace Smith")]
    public void OffsetTS_OrderByStringPropertyType_ReturnsPaginated(
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Offset(_users, user => user.Name, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Name, Is.EqualTo(expectedName));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, 0, 12, 12, null)]
    [TestCase(SortingDirection.Ascending, 7, 5, 5, "   ")]
    [TestCase(SortingDirection.Descending, 0, 12, 12, "789 Oak St")]
    [TestCase(SortingDirection.Descending, 2, 5, 5, "456 Elm St")]
    public void OffsetTS_OrderByNestedPropertyType_ReturnsPaginated(
        SortingDirection sortingDirection,
        int skip,
        int take,
        int expectedCount,
        string expectedStreet)
    {
        // Act
        var page = PaginationHelper.Offset(_users, user => user.Address.Street, sortingDirection, skip, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Address?.Street, Is.EqualTo(expectedStreet));
    }

    [Test]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 11, 1, 1, 12)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 2, 5, 5, 3)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 5, 10, 7, 6)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 0, 10, 10, 1)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 11, 1, 1, 10)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 6, 5, 5, 5)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 4, 5, 3, 3)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 12, 5, 5, 11)]
    public void KeysetT_OrderByIntPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        object key,
        int take,
        int expectedCount,
        int expectedId)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, sortingProperty, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Id, Is.EqualTo(expectedId));
    }

    [Test]
    [TestCase(nameof(User.SubscriptionId), SortingDirection.Ascending, 111555, 1, 1, 3125674)]
    // [TestCase(nameof(User.Id), SortingDirection.Descending, 11, 1, 1, 10)]
    public void KeysetT_OrderByNullableIntPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int? key,
        int take,
        int expectedCount,
        int expectedSubscriptionId)
    {
        var page = PaginationHelper.Keyset(_users, sortingProperty, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().SubscriptionId, Is.EqualTo(expectedSubscriptionId));
    }

    [Test]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Alice Smith", 10, 8, "Bob Johnson")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Bob Johnson", 5, 5, "Charlie Brown")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Bob Johnson", 5, 5, "Charlie Brown")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Charlie Brown", 1, 1, "Dana White")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Frank Oldman", 5, 3, "Grace Smith")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, "John Wick", 5, 5, "Hannah Montana The Third of Her Name")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, "Alice Smith", 5, 2, "")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, "Hannah Montana The Third of Her Name", 20, 10, "Grace Smith")]
    public void KeysetT_OrderByStringPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        object key,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, sortingProperty, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Name, Is.EqualTo(expectedName));
    }

    [Test]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Ascending, "Shelbyville", 10, 1, "Springfield")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Ascending, "North Haverbrook", 10, 4, "Ogdenville")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Ascending, "Ogdenville", 2, 2, "Shelbyville")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Descending, "Shelbyville", 10, 9, "Ogdenville")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Descending, "North Haverbrook", 10, 7, null)]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Descending, "North Haverbrook", 5, 5, null)]
    public void KeysetT_OrderByNestedPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        object key,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, sortingProperty, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Address?.City, Is.EqualTo(expectedName));
    }

    [Test]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 11, 1, 1, 12)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 2, 5, 5, 3)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 5, 10, 7, 6)]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 0, 10, 10, 1)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 11, 1, 1, 10)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 6, 5, 5, 5)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 4, 5, 3, 3)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 12, 5, 5, 11)]
    public void KeysetTS_OrderByIntPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int key,
        int take,
        int expectedCount,
        int expectedId)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, sortingProperty, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Id, Is.EqualTo(expectedId));
    }
    
    [Test]
    [TestCase(nameof(User.SubscriptionId), SortingDirection.Ascending, 111555, 1, 1, 3125674)]
    [TestCase(nameof(User.SubscriptionId), SortingDirection.Ascending, 111555, 10, 2, 3125674)]
    [TestCase(nameof(User.SubscriptionId), SortingDirection.Descending, 3125674, 1, 1, 111555)]
    [TestCase(nameof(User.SubscriptionId), SortingDirection.Descending, 3125674, 10, 1, 111555)]
    public void KeysetTS_OrderByNullableIntPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        int key,
        int take,
        int expectedCount,
        int expectedSubscriptionId)
    {
        // Arrange
        int? nullableInt = key;

        // Act
        var page = PaginationHelper.Keyset(_users, sortingProperty, nullableInt, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().SubscriptionId, Is.EqualTo(expectedSubscriptionId));
    }

    [Test]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Alice Smith", 10, 8, "Bob Johnson")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Bob Johnson", 5, 5, "Charlie Brown")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Bob Johnson", 5, 5, "Charlie Brown")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Charlie Brown", 1, 1, "Dana White")]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, "Frank Oldman", 5, 3, "Grace Smith")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, "John Wick", 5, 5, "Hannah Montana The Third of Her Name")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, "Alice Smith", 5, 2, "")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, "Hannah Montana The Third of Her Name", 20, 10, "Grace Smith")]
    public void KeysetTS_OrderByStringPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        string key,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, sortingProperty, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Name, Is.EqualTo(expectedName));
    }

    [Test]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Ascending, "Shelbyville", 10, 1, "Springfield")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Ascending, "North Haverbrook", 10, 4, "Ogdenville")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Ascending, "Ogdenville", 2, 2, "Shelbyville")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Descending, "Shelbyville", 10, 9, "Ogdenville")]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Descending, "North Haverbrook", 10, 7, null)]
    [TestCase($"{nameof(User.Address)}.{nameof(User.Address.City)}", SortingDirection.Descending, "North Haverbrook", 5, 5, null)]
    public void KeysetTS_OrderByNestedPropertyType_ReturnsPaginated(
        string sortingProperty,
        SortingDirection sortingDirection,
        string key,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, sortingProperty, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Address?.City, Is.EqualTo(expectedName));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, 11, 1, 1, 12)]
    [TestCase(SortingDirection.Ascending, 2, 5, 5, 3)]
    [TestCase(SortingDirection.Ascending, 5, 10, 7, 6)]
    [TestCase(SortingDirection.Ascending, 0, 10, 10, 1)]
    [TestCase(SortingDirection.Descending, 11, 1, 1, 10)]
    [TestCase(SortingDirection.Descending, 6, 5, 5, 5)]
    [TestCase(SortingDirection.Descending, 4, 5, 3, 3)]
    [TestCase(SortingDirection.Descending, 12, 5, 5, 11)]
    public void KeysetTS_Expression_OrderByIntPropertyType_ReturnsPaginated(
        SortingDirection sortingDirection,
        int key,
        int take,
        int expectedCount,
        int expectedId)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, x => x.Id, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Id, Is.EqualTo(expectedId));
    }
    
    [Test]
    [TestCase(SortingDirection.Ascending, 111555, 1, 1, 3125674)]
    [TestCase(SortingDirection.Ascending, 111555, 10, 2, 3125674)]
    [TestCase(SortingDirection.Descending, 3125674, 1, 1, 111555)]
    [TestCase(SortingDirection.Descending, 3125674, 10, 1, 111555)]
    public void KeysetTS_Expression_OrderByNullableIntPropertyType_ReturnsPaginated(
        SortingDirection sortingDirection,
        int? key,
        int take,
        int expectedCount,
        int expectedSubscriptionId)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, x => x.SubscriptionId, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().SubscriptionId, Is.EqualTo(expectedSubscriptionId));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, "Alice Smith", 10, 8, "Bob Johnson")]
    [TestCase(SortingDirection.Ascending, "Bob Johnson", 5, 5, "Charlie Brown")]
    [TestCase(SortingDirection.Ascending, "Bob Johnson", 5, 5, "Charlie Brown")]
    [TestCase(SortingDirection.Ascending, "Charlie Brown", 1, 1, "Dana White")]
    [TestCase(SortingDirection.Ascending, "Frank Oldman", 5, 3, "Grace Smith")]
    [TestCase(SortingDirection.Descending, "John Wick", 5, 5, "Hannah Montana The Third of Her Name")]
    [TestCase(SortingDirection.Descending, "Alice Smith", 5, 2, "")]
    [TestCase(SortingDirection.Descending, "Hannah Montana The Third of Her Name", 20, 10, "Grace Smith")]
    public void KeysetTS_Expression_OrderByStringPropertyType_ReturnsPaginated(
        SortingDirection sortingDirection,
        string key,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, x => x.Name, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Name, Is.EqualTo(expectedName));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, "Shelbyville", 10, 1, "Springfield")]
    [TestCase(SortingDirection.Ascending, "North Haverbrook", 10, 4, "Ogdenville")]
    [TestCase(SortingDirection.Ascending, "Ogdenville", 2, 2, "Shelbyville")]
    [TestCase(SortingDirection.Descending, "Shelbyville", 10, 9, "Ogdenville")]
    [TestCase(SortingDirection.Descending, "North Haverbrook", 10, 7, null)]
    [TestCase(SortingDirection.Descending, "North Haverbrook", 5, 5, null)]
    public void KeysetTS_Expression_OrderByNestedPropertyType_ReturnsPaginated(
        SortingDirection sortingDirection,
        string key,
        int take,
        int expectedCount,
        string expectedName)
    {
        // Act
        var page = PaginationHelper.Keyset(_users, x => x.Address.City, key, sortingDirection, take).ToList();

        // Assert
        Assert.That(actual: page.Count, Is.EqualTo(expectedCount));
        Assert.That(actual: page.First().Address?.City, Is.EqualTo(expectedName));
    }
}