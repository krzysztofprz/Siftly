using System;
using System.Linq;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using Siftly.UnitTests.Model;

namespace Siftly.Benchmark.FilteringHelper;

[MemoryDiagnoser]
public class FilteringHelperBenchmark
{
    [GlobalSetup]
    public void Setup() => TestConfiguration.Setup();

    [Benchmark]
    public void SiftlyFilter()
    {
        var result = Helpers.Queryable.FilteringHelper
            .Filter(TestConfiguration.Users, nameof(User.Id), TestConfiguration.Id).ToList();
    }

    [Benchmark]
    public void LinqFilter()
    {
        var result = TestConfiguration.Users.Where(x => x.Id == TestConfiguration.Id).ToList();
    }
}