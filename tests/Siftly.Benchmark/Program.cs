using System;
using System.Linq;
using AutoFixture;
using BenchmarkDotNet.Running;
using Siftly.Benchmark.FilteringHelper;
using Siftly.Benchmark.PaginationHelper;
using Siftly.Benchmark.SortingHelper;
using Siftly.UnitTests.Model;

var filtering = BenchmarkRunner.Run<FilteringHelperBenchmark>();
var sorting = BenchmarkRunner.Run<SortingHelperBenchmark>();
var pagination = BenchmarkRunner.Run<PaginationHelperBenchmark>();

public static class TestConfiguration
{
    public const int Id = 1000;
    public static IQueryable<User> Users;

    public static void Setup()
    {
        var fixture = new Fixture();
        int id = 1;
        fixture.Customize<User>(x => x
            .With(p => p.Id, () => id++)
            .With(p => p.Name, () => $"User_{Guid.NewGuid()}"));

        Users = fixture.CreateMany<User>(2000)
            .ToList()
            .AsQueryable();
    }
}