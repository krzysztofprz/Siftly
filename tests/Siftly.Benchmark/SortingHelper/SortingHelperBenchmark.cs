using System.Linq;
using BenchmarkDotNet.Attributes;
using Siftly.UnitTests.Model;

namespace Siftly.Benchmark.SortingHelper;

public class SortingHelperBenchmark
{
    // [Benchmark]
    // public void SiftlySorting()
    // {
    //     var result = Helpers.Queryable.FilteringHelper
    //         .Filter(TestConfiguration._users, nameof(User.Id), TestConfiguration._id).ToList();
    // }
    //
    // [Benchmark]
    // public void LinqSorting()
    // {
    //     var result = TestConfiguration._users.OrderBy(x => x.Id).ToList();
    // }
}