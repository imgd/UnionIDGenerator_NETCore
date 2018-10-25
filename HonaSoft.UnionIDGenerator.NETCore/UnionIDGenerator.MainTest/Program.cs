using BenchmarkDotNet.Running;
using System;
using UnionIDGenerator.Test;

namespace UnionIDGenerator.MainTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<SnowflakeTest>();
            var summary2 = BenchmarkRunner.Run<SnowflakeTest2>();
            Console.ReadKey();
        }
    }
}
