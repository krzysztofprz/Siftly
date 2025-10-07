# Siftly

Siftly is a lightweight .NET library for dynamic _filtering_, _sorting_, and _pagination_ of generic collections. Using LINQ-to-SQL (IQueryable<T>) queries works with Entity Framework and other LINQ providers.

## Usage

```csharp
using Siftly;
using Siftly.Model;

// Example data (works the same for DbSet<User> in EF)
var users = new List<User>
{
    new User { Id = 1, FirstName = "John", LastName = "Smith", Age = 30 },
    new User { Id = 2, FirstName = "Jane", LastName = "Doe", Age = 25 },
    // ...
}.AsQueryable();

// Filtering by property name (case-insensitive)
var filtered = FilteringHelper.Filter(users, "FirstName", "John");

// Filtering with strong typing
var filteredTyped = FilteringHelper.Filter(users, "Age", 30);

// Sorting by property name
var sorted = SortingHelper.Sort(users, "LastName", SortingDirection.Ascending);

// Sorting with expression
var sortedExpr = SortingHelper.Sort(users, u => u.Age, SortingDirection.Descending);

// Offset pagination
var paged = PaginationHelper.Offset(users, "Id", SortingDirection.Ascending, skip: 10, take: 20);

// Keyset pagination
// Assume lastSeenId is defined
var keysetPaged = PaginationHelper.Keyset(users, "Id", lastSeenId, SortingDirection.Ascending, take: 20);


// Usage with Entity Framework (DbContext)
// var filteredDb = FilteringHelper.Filter(dbContext.Users, "FirstName", "John");