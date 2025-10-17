using System.Linq;
using BenchmarkDotNet.Attributes;
using Siftly.Model;
using Siftly.UnitTests.Model;

namespace Siftly.Benchmark.PaginationHelper;

[MemoryDiagnoser]
public class PaginationHelperBenchmark
{
    [GlobalSetup]
    public void Setup() => TestConfiguration.Setup();

    [Benchmark]
    public void SiftlyOffsetPaginationByPropertyName()
    {
        var result = Helpers.Queryable.PaginationHelper
            .Offset(TestConfiguration.Users, nameof(User.Id), SortingDirection.Ascending, 10, 10).ToList();
    }

    [Benchmark]
    public void SiftlyOffsetPaginationByExpression()
    {
        var result = Helpers.Queryable.PaginationHelper
            .Offset(TestConfiguration.Users, x => x.Id, SortingDirection.Ascending, 10, 10).ToList();
    }

    [Benchmark]
    public void LinqOffestPagination()
    {
        var result = TestConfiguration.Users.OrderBy(x => x.Id).Skip(10).Take(10).ToList();
    }

    [Benchmark]
    public void SiftlyKeysetPaginationByPropertyName()
    {
        var result = Helpers.Queryable.PaginationHelper
            .Keyset(TestConfiguration.Users, nameof(User.Id), TestConfiguration.Id, SortingDirection.Ascending, 10).ToList();
    }

    [Benchmark]
    public void SiftlyKeysetPaginationByExpression()
    {
        var result = Helpers.Queryable.PaginationHelper
            .Keyset(TestConfiguration.Users, x => x.Id, TestConfiguration.Id, SortingDirection.Ascending, 10).ToList();
    }

    [Benchmark]
    public void LinqKeysetPagination()
    {
        var result = TestConfiguration.Users
            .Where(x => x.Id > TestConfiguration.Id)
            .OrderBy(x => x.Id)
            .Take(10)
            .ToList();
    }
}