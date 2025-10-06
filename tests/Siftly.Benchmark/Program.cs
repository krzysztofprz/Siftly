// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Siftly.Benchmark.FilteringHelper;

//var filtering = BenchmarkRunner.Run<FilteringHelperBenchmark>(new DebugInProcessConfig());
if (Debugger.IsAttached)
    BenchmarkRunner.Run<FilteringHelperBenchmark>(new DebugInProcessConfig());
else
    BenchmarkRunner.Run<FilteringHelperBenchmark>();
// var pagination = BenchmarkRunner.Run<PaginationHelperBenchmark>();