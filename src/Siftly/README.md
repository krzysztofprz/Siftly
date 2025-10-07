# Siftly

Siftly is a lightweight .NET library for dynamic _filtering_, _sorting_, and _pagination_ of generic collections. It enables runtime construction of LINQ-to-SQL (`IQueryable<T>`) queries using property names or expressions. Siftly is built for APIs and data access layers that require user-driven or dynamic querying, and works with Entity Framework and other LINQ providers. Siftly methods are nearly as fast as direct LINQ, with minimal overhead for dynamic property access.

---

## Features

- **Dynamic Filtering:** Filter collections or database queries by property names or strongly-typed expressions.
- **Dynamic Sorting:** Sort by property names or expressions, supporting nested properties.
- **Offset & Keyset Pagination:** Efficiently page through results, including support for keyset (seek) pagination.
- **Database Ready:** Designed to work with `IQueryable<T>`, making it suitable for Entity Framework and other ORMs.

---

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
```

---

## Benchmark Results

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.6725/24H2/2024Update/HudsonValley)

Processor	AMD Ryzen 5 7535HS with Radeon Graphics, 3301 Mhz, 6 Core(s), 12 Logical Processor(s)

.NET SDK 8.0.414
  [Host]     : .NET 8.0.20 (8.0.2025.41914), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.20 (8.0.2025.41914), X64 RyuJIT AVX2

```
| Method       | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|------------- |---------:|--------:|--------:|-------:|-------:|----------:|
| SiftlyFilter | 240.2 μs | 2.71 μs | 2.26 μs | 0.9766 | 0.4883 |  11.48 KB |
| LinqFilter   | 221.0 μs | 2.41 μs | 2.01 μs | 0.9766 | 0.4883 |  11.04 KB |
```

```
| Method        | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|-------------- |---------:|--------:|--------:|-------:|-------:|----------:|
| SiftlySorting | 348.8 μs | 3.71 μs | 3.29 μs | 8.7891 | 3.9063 |  74.68 KB |
| LinqSorting   | 322.7 μs | 2.98 μs | 2.49 μs | 8.7891 | 4.3945 |   74.2 KB |
```

```
| Method                 | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|----------------------- |---------:|--------:|--------:|-------:|-------:|----------:|
| SiftlyOffsetPagination | 360.7 μs | 1.16 μs | 1.03 μs | 5.3711 | 2.4414 |  46.63 KB |
| LinqOffestPagination   | 334.9 μs | 1.20 μs | 1.07 μs | 5.3711 | 2.4414 |  46.15 KB |
| SiftlyKeysetPagination | 516.4 μs | 1.82 μs | 1.52 μs | 5.8594 | 4.8828 |  52.17 KB |
| LinqKeysetPagination   | 528.8 μs | 1.97 μs | 1.64 μs | 4.8828 | 3.9063 |  43.87 KB |
```

*Benchmarks run on 2,000 user objects, .NET 8.0, Release mode.*

---

## Installation

*(To be added once published to NuGet)*

---
## Future Development

- **Filtering with expression:**
  ```csharp
  var filtered = FilteringHelper.Filter(users, u => u.FirstName, "John");
  ```
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
