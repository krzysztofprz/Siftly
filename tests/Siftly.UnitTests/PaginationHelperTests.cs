using Siftly.Helpers;
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
        //var page = PaginationHelper.Offset(_users, user => new { user.Id, user.Name }, sortingDirection, skip, take).ToList();

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
}