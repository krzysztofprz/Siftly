using System.Linq;
using BenchmarkDotNet.Attributes;
using Siftly.UnitTests.Model;

namespace Siftly.Benchmark.FilteringHelper;

[MemoryDiagnoser]
public class FilteringHelperBenchmark
{
    [GlobalSetup]
    public void Setup() => TestConfiguration.Setup();

    [Benchmark]
    public void SiftlyFilterByPropertyName()
    {
        var result = Helpers.Queryable.FilteringHelper
            .Filter(TestConfiguration.Users, nameof(User.Id), TestConfiguration.Id).ToList();
    }

    [Benchmark]
    public void SiftlyFilterByExpression()
    {
        var result = Helpers.Queryable.FilteringHelper
            .Filter(TestConfiguration.Users, x => x.Id, TestConfiguration.Id).ToList();
    }

    [Benchmark]
    public void LinqFilter()
    {
        var result = TestConfiguration.Users.Where(x => x.Id == TestConfiguration.Id).ToList();
    }
}