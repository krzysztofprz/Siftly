# Siftly

Siftly is a lightweight .NET library for dynamic _filtering_, _sorting_, and _pagination_ of generic collections. It enables runtime construction of LINQ-to-SQL (`IQueryable<T>`) queries using property names or expressions. Siftly is built for APIs and data access layers that require user-driven or dynamic querying, and works with Entity Framework and other LINQ providers. [Siftly methods are nearly as fast as direct LINQ](#benchmark-results), with minimal overhead for dynamic property access.
<br>
If you find this project useful, please consider giving it a ⭐ on GitHub!

---


## Features

- **Dynamic Filtering:** Filter collections or database queries by property names or strongly-typed expressions.
- **Dynamic Sorting:** Sort by property names or expressions, supporting nested properties.
- **Offset & Keyset Pagination:** Efficiently page through results, including support for keyset (seek) pagination.
- **Database Ready:** Designed to work with `IQueryable<T>`, making it suitable for Entity Framework and other ORMs.

---

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

---

## Benchmark Results

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.6725/24H2/2024Update/HudsonValley)

Processor	AMD Ryzen 5 7535HS with Radeon Graphics, 3301 Mhz, 6 Core(s), 12 Logical Processor(s)

.NET SDK 8.0.414
  [Host]     : .NET 8.0.20 (8.0.2025.41914), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.20 (8.0.2025.41914), X64 RyuJIT AVX2

```
| Method                     | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|--------------------------- |---------:|--------:|--------:|-------:|-------:|----------:|
| SiftlyFilterByPropertyName | 236.8 μs | 4.27 μs | 3.79 μs | 0.9766 | 0.4883 |  11.48 KB |
| SiftlyFilterByExpression   | 222.2 μs | 3.10 μs | 2.42 μs | 0.9766 | 0.4883 |  11.31 KB |
| LinqFilter                 | 217.9 μs | 3.89 μs | 3.03 μs | 0.9766 | 0.4883 |  10.92 KB |
```

```
| Method                      | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|---------------------------- |---------:|--------:|--------:|-------:|-------:|----------:|
| SiftlySortingByPropertyName | 341.0 μs | 4.87 μs | 4.06 μs | 8.7891 | 3.9063 |  74.68 KB |
| SiftlySortingByExpression   | 318.7 μs | 2.62 μs | 2.19 μs | 8.7891 | 4.3945 |  74.79 KB |
| LinqSorting                 | 319.9 μs | 4.11 μs | 3.21 μs | 8.7891 | 4.3945 |   74.2 KB |
```

```
| Method                               | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------------------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| SiftlyOffsetPaginationByPropertyName | 358.9 μs |  6.37 μs |  5.95 μs | 5.3711 | 2.4414 |  46.63 KB |
| SiftlyOffsetPaginationByExpression   | 330.3 μs |  1.22 μs |  1.08 μs | 5.3711 | 2.4414 |  46.53 KB |
| LinqOffestPagination                 | 393.9 μs | 19.34 μs | 57.03 μs | 5.3711 | 2.4414 |  46.17 KB |
| SiftlyKeysetPaginationByPropertyName | 519.6 μs |  9.41 μs |  7.85 μs | 5.8594 | 4.8828 |  52.17 KB |
| SiftlyKeysetPaginationByExpression   | 501.9 μs |  3.78 μs |  3.16 μs | 5.8594 | 4.8828 |  52.26 KB |
| LinqKeysetPagination                 | 520.8 μs |  2.47 μs |  2.06 μs | 4.8828 | 3.9063 |  43.87 KB |
```

*Benchmarks run on 2,000 user objects, .NET 8.0, Release mode.*

---

## Installation

.NET CLI
```
dotnet add package Siftly --version 1.1.0
```

Project package reference
```
<PackageReference Include="Siftly" Version="1.1.0" />
```


---
## Future Development

- **Multi-Property Filtering:**  
  Support for filtering on multiple properties at once, e.g.:
  ```csharp
  FilteringHelper.Filter(users, new[] { "FirstName", "LastName" }, new[] { "John", "Smith" });
  ```
- **Multi-Property Sorting:**  
  Allow sorting by multiple fields, e.g.:
  ```csharp
  SortingHelper.Sort(users, new[] { "LastName", "FirstName" });
  ```
- **Multi-Property Pagination:**  
  Enable keyset pagination using composite keys.
  
---

## License

MIT
