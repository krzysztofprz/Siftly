### What this repository is

Siftly is a small .NET library (netstandard2.0 target) that provides dynamic filtering, sorting and pagination helpers that operate on IQueryable<T>. The core helpers live under `src/Siftly/Helpers/Queryable` and include:

- `FilteringHelper.cs` — build runtime Expression<Func<T,bool>> filters from property names or expressions.
- `SortingHelper.cs` — build OrderBy/OrderByDescending calls from property names or expressions; includes null-safe member access.
- `PaginationHelper.cs` — provides offset and keyset pagination helpers that compose `SortingHelper` + `Where`/`Skip`/`Take`.

Unit tests and benchmarks live under `tests/` and are the best quick references for correct usage and edge cases (see `tests/Siftly.UnitTests` and `tests/Siftly.Benchmark`).

### Key patterns and conventions for edits

- API surface favors static helper methods that accept either a string property path (case-insensitive, supports nesting like `Address.Street`) or a strongly-typed expression `Expression<Func<T,S>>`.
- When using string property paths, the code resolves nested properties via `ExpressionExtensions.GetNestedProperty` (look there for null-safety behavior and a per-(Type,name) cache).
- Many public methods validate arguments and throw `ArgumentNullException` or `ArgumentException`; keep that behavior when adding new helpers.
- Use `Expression.Parameter(typeof(T), ExpressionExtensions.Arg)` when constructing expressions so generated lambdas match the project's param name conventions.
- For string comparisons in keyset pagination, the code calls `string.Compare` (see `PaginationHelper.GetCompare`) — be careful to preserve this behavior when refactoring.

### Files to inspect for context or examples

- `src/Siftly/Helpers/Queryable/FilteringHelper.cs`
- `src/Siftly/Helpers/Queryable/SortingHelper.cs`
- `src/Siftly/Helpers/Queryable/PaginationHelper.cs`
- `src/Siftly/Extensions/ExpressionExtensions.cs` (property resolution + Arg constant)
- `src/Siftly/Model/SortingDirection.cs` (enum values used across helpers)
- `tests/Siftly.UnitTests/*` — concrete usage examples and edge-case tests (filtering, sorting, pagination)

### Developer workflows

- Build & tests: uses .NET CLI. Typical commands (Windows PowerShell):

```powershell
# restore, build and run unit tests
dotnet restore
dotnet build -c Release
dotnet test
```

- Benchmarks: there is a Benchmark project under `tests/Siftly.Benchmark`; run it with `dotnet run -c Release --project tests/Siftly.Benchmark`.

### What to watch for when changing expression logic

- Null-safety: `GetNestedProperty` and `SortingHelper.NullSafe` intentionally emit conditional expressions to avoid null-reference exceptions when navigating nested properties. Preserving this approach is required for LINQ providers that translate expressions to SQL.
- Caching: `ExpressionExtensions` uses a ConcurrentDictionary keyed by (Type, member) for reflection lookups. If you change the property resolution logic, keep or replace caching to avoid regressions in performance.
- Expression param name: many unit-tests assume the parameter name `x` (via `ExpressionExtensions.Arg`). New expression builders should use the same constant to avoid brittle test failures when expressions are converted to strings in tests.

### Small examples to follow

- Filtering by string property name:

```csharp
var filtered = FilteringHelper.Filter(users, "FirstName", "John");
```

- Sorting with an expression (null-safe nested access is performed internally):

```csharp
var sorted = SortingHelper.Sort(users, u => u.Address.City, SortingDirection.Ascending);
```

- Keyset pagination by property name (string comparison used for string-typed keys):

```csharp
var page = PaginationHelper.Keyset(users, "Id", lastSeenId, SortingDirection.Ascending, take: 20);
```

### Tests and examples to mirror when implementing features

- Use unit test patterns in `tests/Siftly.UnitTests/SortingHelperTests.cs` and `PaginationHelperTests.cs` for input validation and expected behavior (null inputs, nested properties, nullable types).

### When to ask for clarification

- If a requested change affects expression structure (changing how null checks are emitted, parameter names, or the order/composition of Expression.Call/Quote), ask for specifics — these changes can subtly affect LINQ providers (EF) translation and test string comparisons.

If you'd like, I can tailor or expand these instructions to include a short checklist for PR reviewers (tests to run, files to inspect, and common pitfalls to look for).
