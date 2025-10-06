using System;
using System.Linq;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using Siftly.UnitTests.Model;

namespace Siftly.Benchmark.FilteringHelper;

public class FilteringHelperBenchmark
{
    const int _id = 1000;
    static IQueryable<User> _users;

    [GlobalSetup]
    public void Setup()
    {
        var fixture = new Fixture();
        int id = 1;
        fixture.Customize<User>(x => x
            .With(p => p.Id, () => id++)
            .With(p => p.Name, () => $"User_{Guid.NewGuid()}"));

        _users = fixture.CreateMany<User>(2000)
            .ToList()
            .AsQueryable();
    }

    [Benchmark]
    public void SiftlyFilter()
    {
        var result = Helpers.Queryable.FilteringHelper
            .Filter(_users, nameof(User.Id), _id).ToList();
    }

    [Benchmark]
    public void LinqFilter()
    {
        var result = _users.Where(x => x.Id == _id).ToList();
    }
}