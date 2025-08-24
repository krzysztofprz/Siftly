# Siftly
Lightweight 
Siftly is a lightweight .NET library for dynamic filtering, sorting, and pagination on generic collections.
- **Filtering** by property name (case-insensitive)  
- **Sorting** in ascending or descending order  
- **Pagination** with both offset and keyset strategies  

It works seamlessly with both in-memory collections (`IEnumerable<T>`) and database queries (`IQueryable<T>`), making it useful for building flexible APIs and data access layers.

## Features
- Dynamic filtering with strongly-typed and object-based overloads
- Sorting by property names or expression selectors
- Offset and keyset pagination helpers
- LINQ-based implementation using reflection and expression trees

## Installation
*(To be added once published to NuGet)*

## Usage
```csharp
using Siftly;

// Filtering
var filtered = EnumerableHelper.Filter(users, "Name", "John");

// Sorting
var sorted = QueryableHelper.Sort(users.AsQueryable(), "Age", SortingDirection.Descending);

// Pagination
var paged = PaginationHelper.Offset(users.AsQueryable(), "Id", SortingDirection.Ascending, skip: 10, take: 20);
