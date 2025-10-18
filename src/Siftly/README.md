# Siftly

Siftly is a lightweight .NET library for dynamic _filtering_, _sorting_, and _pagination_ of generic collections. Using LINQ-to-SQL (IQueryable<T>) queries works with Entity Framework and other LINQ providers.

## Usage

### Class method
```csharp
using Siftly;

// Example data (works the same for DbSet<User> in EF)
var users = new List<User>
{
    new User { Id = 1, FirstName = "John", LastName = "Smith", Age = 30 },
    new User { Id = 2, FirstName = "Jane", LastName = "Doe", Age = 25 },
    // ...
}.AsQueryable();

// Filtering by property name (case-insensitive)
var filtered = FilteringHelper.Filter(users, "FirstName", "John");

// Filtering with expression
var filteredExpr = FilteringHelper.Filter(users, u => u.FirstName, "John");

// Filtering with strong typing
var filteredTyped = FilteringHelper.Filter(users, "Age", 30);

// Sorting by property name
var sorted = SortingHelper.Sort(users, "LastName", SortingDirection.Ascending);

// Sorting with expression
var sortedExpr = SortingHelper.Sort(users, u => u.LastName, SortingDirection.Descending);

// Offset pagination
var paged = PaginationHelper.Offset(users, "Id", SortingDirection.Ascending, skip: 10, take: 20);

// Offset pagination with expression
var pagedExpr = PaginationHelper.Offset(users, u => u.Id, SortingDirection.Ascending, skip: 10, take: 20);

// Keyset pagination
// Assume lastSeenId is defined
var keysetPaged = PaginationHelper.Keyset(users, "Id", lastSeenId, SortingDirection.Ascending, take: 20);

// Keyset pagination with expression
// Assume lastSeenId is defined
var keysetPagedExpr = PaginationHelper.Keyset(users, u => u.Id, lastSeenId, SortingDirection.Ascending, take: 20);

// Usage with Entity Framework (DbContext)
// var filteredDb = FilteringHelper.Filter(dbContext.Users, "FirstName", "John");
```

### Extension method
```csharp
using Siftly.Extensions;

// Example data (works the same for DbSet<User> in EF)
var users = new List<User>
{
    new User { Id = 1, FirstName = "John", LastName = "Smith", Age = 30 },
    new User { Id = 2, FirstName = "Jane", LastName = "Doe", Age = 25 },
    // ...
}.AsQueryable();

// Filtering by property name (case-insensitive)
var filteredExtension = users.Filter("FirstName", "John");

// Filtering with expression
var filteredExtensionExpr = users.Filter(u => u.FirstName, "John");

// Filtering with strong typing
var filteredExtensionTyped = users.Filter("Age", 30);

// Sorting by property name
var sortedExtension = users.Sort("LastName", SortingDirection.Ascending);

// Sorting with expression
var sortedExtensionExpr = users.Sort(u => u.LastName, SortingDirection.Ascending);

// Offset pagination
var pagedExtensions = users.Offset("Id", SortingDirection.Ascending, skip: 10, take: 20);

// Offset pagination with expression
var pagedExtensionExpr = users.Offset(u => u.Id, SortingDirection.Ascending, skip: 10, take: 20);

// Keyset pagination
// Assume lastSeenId is defined
var keysetPagedExtension = users.Keyset("Id", lastSeenId, SortingDirection.Ascending, take: 20);

// Keyset pagination with expression
// Assume lastSeenId is defined
var keysetPagedExtensionExpr = users.Keyset(u => u.Id, lastSeenId, SortingDirection.Ascending, take: 20);

// Usage with Entity Framework (DbContext)
// var filteredDb = dbContext.Users.Filter("FirstName", "John");
```