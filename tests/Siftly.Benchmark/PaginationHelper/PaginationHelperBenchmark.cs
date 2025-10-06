using System.Linq;
using BenchmarkDotNet.Attributes;
using Siftly.UnitTests.Model;

namespace Siftly.Benchmark.PaginationHelper;

public class PaginationHelperBenchmark
{
    // //[Benchmark]
    // public void SiftlyPagination()
    // {
    //     var result = Helpers.Queryable.FilteringHelper
    //         .Filter(TestConfiguration._users, nameof(User.Id), TestConfiguration._id).ToList();
    // }
    //
    // //[Benchmark]
    // public void LinqPagination()
    // {
    //     var result = TestConfiguration._users.Where(x => x.Id == TestConfiguration._id).ToList();
    // }
}