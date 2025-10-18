using Siftly.Helpers.Queryable;
using Siftly.Model;
using Siftly.UnitTests.Helpers;
using Siftly.UnitTests.Model;

namespace Siftly.UnitTests;

[TestFixture]
public class SortingHelperTests
{
    private IQueryable<User> _users;

    [SetUp]
    public void Setup()
    {
        _users = UserTestDataHelper.GetUsers().AsQueryable();
    }

    [Test]
    [TestCase(nameof(User.Id), SortingDirection.Ascending, 1, 12)]
    [TestCase(nameof(User.Id), SortingDirection.Descending, 12, 1)]
    public void SortT_IntPropertyType_IdProperty_ReturnsSorted(
        string sortingProperty,
        SortingDirection sortingDirection,
        int expectedFirst,
        int expectedLast)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, sortingProperty, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First().Id, Is.EqualTo(expectedFirst));
        Assert.That(actual: sorted.Last().Id, Is.EqualTo(expectedLast));
    }

    [Test]
    [TestCase(nameof(User.SubscriptionId), SortingDirection.Ascending, null)]
    [TestCase(nameof(User.SubscriptionId), SortingDirection.Descending, 3125674)]
    public void SortT_NullableIntPropertyType_SubscriptionIdProperty_ReturnsSorted(
        string sortingProperty,
        SortingDirection sortingDirection,
        int? expectedFirst)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, sortingProperty, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First().SubscriptionId, Is.EqualTo(expectedFirst));
    }

    [Test]
    [TestCase(nameof(User.Name), SortingDirection.Ascending, null, "John Wick")]
    [TestCase(nameof(User.Name), SortingDirection.Descending, "John Wick", null)]
    public void SortT_StringPropertyType_NameProperty_ReturnsSorted(
        string sortingProperty,
        SortingDirection sortingDirection,
        string expectedFirst,
        string expectedLast)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, sortingProperty, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First().Name, Is.EqualTo(expectedFirst));
        Assert.That(actual: sorted.Last().Name, Is.EqualTo(expectedLast));
    }

    [Test]
    public void SortT_DateTimePropertyType_LastLoginProperty_Ascending_ReturnsSorted()
    {
        // Act
        var sorted = SortingHelper.Sort(_users, nameof(User.LastLogin)).ToList();

        // Assert
        Assert.That(actual: sorted.First().LastLogin, Is.EqualTo(null));
        Assert.That(actual: sorted.Last().LastLogin!.Value.Date, Is.EqualTo(DateTime.UtcNow.Date));
    }

    [Test]
    public void SortT_DateTimePropertyType_LastLoginProperty_Descending_ReturnsSorted()
    {
        // Act
        var sorted = SortingHelper.Sort(_users, nameof(User.LastLogin), SortingDirection.Descending).ToList();

        // Assert
        Assert.That(actual: sorted.First().LastLogin!.Value.Date, Is.EqualTo(DateTime.UtcNow.Date));
        Assert.That(actual: sorted.Last().LastLogin, Is.EqualTo(null));
    }

    [Test]
    [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", SortingDirection.Ascending, null, "Springfield")]
    [TestCase($"{nameof(User.Address)}.{nameof(Address.City)}", SortingDirection.Descending, "Springfield", null)]
    public void SortT_NestedPropertyType_AddressCityProperty_ReturnsSorted(
        string sortingProperty,
        SortingDirection sortingDirection,
        string expectedFirst,
        string expectedLast)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, sortingProperty, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First()?.Address?.City, Is.EqualTo(expectedFirst));
        Assert.That(actual: sorted.Last()?.Address?.City, Is.EqualTo(expectedLast));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, 1, 12)]
    [TestCase(SortingDirection.Descending, 12, 1)]
    public void SortTS_IntPropertyType_IdProperty_ReturnsSorted(
        SortingDirection sortingDirection,
        int expectedFirst,
        int expectedLast)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, user => user.Id, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First().Id, Is.EqualTo(expectedFirst));
        Assert.That(actual: sorted.Last().Id, Is.EqualTo(expectedLast));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, null)]
    [TestCase(SortingDirection.Descending, 3125674)]
    public void SortTS_NullableIntPropertyType_SubscriptionIdProperty_ReturnsSorted(
        SortingDirection sortingDirection,
        int? expectedFirst)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, user => user.SubscriptionId, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First().SubscriptionId, Is.EqualTo(expectedFirst));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, null, "John Wick")]
    [TestCase(SortingDirection.Descending, "John Wick", null)]
    public void SortTS_StringPropertyType_NameProperty_ReturnsSorted(
        SortingDirection sortingDirection,
        string expectedFirst,
        string expectedLast)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, user => user.Name, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First().Name, Is.EqualTo(expectedFirst));
        Assert.That(actual: sorted.Last().Name, Is.EqualTo(expectedLast));
    }

    [Test]
    public void SortTS_DateTimePropertyType_LastLoginProperty_Ascending_ReturnsSorted()
    {
        // Act
        var sorted = SortingHelper.Sort(_users, user => user.LastLogin).ToList();

        // Assert
        Assert.That(actual: sorted.First().LastLogin, Is.EqualTo(null));
        Assert.That(actual: sorted.Last().LastLogin!.Value.Date, Is.EqualTo(DateTime.UtcNow.Date));
    }

    [Test]
    public void SortTS_DateTimePropertyType_LastLoginProperty_Descending_ReturnsSorted()
    {
        // Act
        var sorted = SortingHelper.Sort(_users, user => user.LastLogin, SortingDirection.Descending).ToList();

        // Assert
        Assert.That(actual: sorted.First().LastLogin!.Value.Date, Is.EqualTo(DateTime.UtcNow.Date));
        Assert.That(actual: sorted.Last().LastLogin, Is.EqualTo(null));
    }

    [Test]
    [TestCase(SortingDirection.Ascending, null, "Springfield")]
    [TestCase(SortingDirection.Descending, "Springfield", null)]
    public void SortTS_NestedPropertyType_AddressCityProperty_ReturnsSorted(
        SortingDirection sortingDirection,
        string expectedFirst,
        string expectedLast)
    {
        // Act
        var sorted = SortingHelper.Sort(_users, user => user.Address.City, sortingDirection).ToList();

        // Assert
        Assert.That(actual: sorted.First()?.Address?.City, Is.EqualTo(expectedFirst));
        Assert.That(actual: sorted.Last()?.Address?.City, Is.EqualTo(expectedLast));
    }
}