using BenchmarkDotNet.Attributes;
using System;
using UnionIDGenerator.Snowflake;

namespace UnionIDGenerator.Test
{
    [ClrJob, CoreJob]
    public class SnowflakeTest
    {
        private Snowflake_Worker _Snowflake_Worker = Snowflake_Worker.Instance(1, 1);

       // [Benchmark]
        public long CreateID1()
        {
            return _Snowflake_Worker.nextId();
        }


        [Benchmark]
        public long[] CreateID10000()
        {
            var lenth = 10000;
            long[] result = new long[10000];
            for (int i = 0; i < lenth; i++)
            {
                result[i] = _Snowflake_Worker.nextId();
            }

            return result;
        }
        [Benchmark]
        public long[] CreateID100000()
        {
            var lenth = 100000;
            long[] result = new long[100000];
            for (int i = 0; i < lenth; i++)
            {
                result[i] = _Snowflake_Worker.nextId();
            }

            return result;
        }
        [Benchmark]
        public long[] CreateID1000000()
        {
            var lenth = 1000000;
            long[] result = new long[1000000];
            for (int i = 0; i < lenth; i++)
            {
                result[i] = _Snowflake_Worker.nextId();
            }

            return result;
        }
    }
}
