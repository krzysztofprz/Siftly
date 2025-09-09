// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Siftly.Helpers;
using Siftly.UnitTests.Model;

var summary = BenchmarkRunner.Run<FilteringHelperBenchmark>();

public class FilteringHelperBenchmark
{
    private readonly int id;
    private readonly IQueryable<User> _users;

    public FilteringHelperBenchmark()
    {
        id = new Random().Next(1, 2000);
        var temp = new List<User>();

        for (int i = 0; i < id; i++)
        {
            temp.Add(new User
            {
                Id = i
            });
        }

        _users = temp.AsQueryable();
    }

    [Benchmark]
    public void SiftlyFilter()
    {
        var result = FilteringHelper.Filter(_users, nameof(User.Id), id).ToList();
    }

    [Benchmark]
    public void LinqFilter()
    {
        var result = _users.Where(x => x.Id == id).ToList();
    }
}