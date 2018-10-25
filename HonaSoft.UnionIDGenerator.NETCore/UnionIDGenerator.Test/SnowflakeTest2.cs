using BenchmarkDotNet.Attributes;
using System;
using UnionIDGenerator.Snowflake;

namespace UnionIDGenerator.Test
{
    [ClrJob, CoreJob]
    public class SnowflakeTest2
    {
        private Snowflake64x _Snowflake_Worker;

        public SnowflakeTest2() {
            Snowflake64x.InitData(1, 1);
            _Snowflake_Worker = Snowflake64x.Instance();
        }

        // [Benchmark]
        public long CreateID1()
        {
            return _Snowflake_Worker.CreateId();
        }


        [Benchmark]
        public long[] CreateID10000()
        {
            
            var lenth = 10000;
            long[] result = new long[10000];
            for (int i = 0; i < lenth; i++)
            {
                result[i] = _Snowflake_Worker.CreateId();
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
                result[i] = _Snowflake_Worker.CreateId();
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
                result[i] = _Snowflake_Worker.CreateId();
            }

            return result;
        }
    }
}
