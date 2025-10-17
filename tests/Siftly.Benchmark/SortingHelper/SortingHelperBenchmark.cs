using System.Linq;
using BenchmarkDotNet.Attributes;
using Siftly.UnitTests.Model;

namespace Siftly.Benchmark.SortingHelper;

[MemoryDiagnoser]
public class SortingHelperBenchmark
{
    [GlobalSetup]
    public void Setup() => TestConfiguration.Setup();

    [Benchmark]
    public void SiftlySortingByPropertyName()
    {
        var result = Helpers.Queryable.SortingHelper
            .Sort(TestConfiguration.Users, nameof(User.Id)).ToList();
    }

    [Benchmark]
    public void SiftlySortingByExpression()
    {
        var result = Helpers.Queryable.SortingHelper
            .Sort(TestConfiguration.Users, x => x.Id).ToList();
    }

    [Benchmark]
    public void LinqSorting()
    {
        var result = TestConfiguration.Users.OrderBy(x => x.Id).ToList();
    }
}