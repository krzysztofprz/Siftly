namespace Siftly.UnitTests
{
	[TestFixture]
	public class QueryableHelperTests
	{
		private IQueryable<User> _testUsers;

		[SetUp]
		public void Setup()
		{
			_testUsers = UserTestDataHelper.GetUsersQueryable();
		}

		[Test]
		public void Filter_ByStringProperty_ReturnsCorrectUser()
		{
			var result = Siftly.QueryableHelper.Filter(_testUsers, "Name", "Alice Smith").ToList();
			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result[0].Name, Is.EqualTo("Alice Smith"));
		}

		[Test]
		public void Filter_ByTypedProperty_ReturnsCorrectUser()
		{
			var result = Siftly.QueryableHelper.Filter<User, int>(_testUsers, "Id", 2).ToList();
			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result[0].Name, Is.EqualTo("Bob Johnson"));
		}

		[Test]
		public void Filter_ByPredicate_ReturnsUsersWithNullLastLogin()
		{
			var result = Siftly.QueryableHelper.Filter(_testUsers, u => u.LastLogin == null).ToList();
			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result.Any(u => u.Name == "Bob Johnson"));
			Assert.That(result.Any(u => u.Name == "Hannah Montana The Third of Her Name"));
		}

		[Test]
		public void Sort_ByName_Ascending_ReturnsSorted()
		{
			var result = Siftly.QueryableHelper.Sort(_testUsers, "Name").ToList();
			var expected = _testUsers.OrderBy(u => u.Name).Select(u => u.Name).ToList();
			Assert.That(result.Select(u => u.Name).ToList(), Is.EqualTo(expected));
		}

		[Test]
		public void Sort_ById_Descending_ReturnsSorted()
		{
			var result = Siftly.QueryableHelper.Sort(_testUsers, "Id", Siftly.Model.SortingDirection.Descending).ToList();
			var expected = _testUsers.OrderByDescending(u => u.Id).Select(u => u.Id).ToList();
			Assert.That(result.Select(u => u.Id).ToList(), Is.EqualTo(expected));
		}

		[Test]
		public void Sort_ByTypedSelector_Ascending_ReturnsSorted()
		{
			var result = Siftly.QueryableHelper.Sort(_testUsers, u => u.DateOfBirth).ToList();
			var expected = _testUsers.OrderBy(u => u.DateOfBirth).Select(u => u.DateOfBirth).ToList();
			Assert.That(result.Select(u => u.DateOfBirth).ToList(), Is.EqualTo(expected));
		}

		[Test]
		public void Sort_ByTypedSelector_Descending_ReturnsSorted()
		{
			var result = Siftly.QueryableHelper.Sort(_testUsers, u => u.DateOfBirth, Siftly.Model.SortingDirection.Descending).ToList();
			var expected = _testUsers.OrderByDescending(u => u.DateOfBirth).Select(u => u.DateOfBirth).ToList();
			Assert.That(result.Select(u => u.DateOfBirth).ToList(), Is.EqualTo(expected));
		}
	}
}
