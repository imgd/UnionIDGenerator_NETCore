using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnionIDGenerator.Snowflake;

namespace UnionIDGenerator.Main
{
    static class Program
    {

        /// <summary>
        /// ID生成类实例
        /// </summary>
        private static Snowflake_Worker _snowflake = Snowflake_Worker.Instance(1, 1);

        /// <summary>
        /// 生成成功条数
        /// </summary>
        private static int _thredsokcount = 0;

        /// <summary>
        /// 分配创建ID线程数
        /// </summary>
        private static readonly int _thredscount = 8;

        /// <summary>
        /// 单线程 执行任务条数
        /// </summary>
        private static readonly int _thredworkcount = 1000000;

        /// <summary>
        /// 待比较的生成数据
        /// </summary>
        private static long[] _worksdata = null;

        /// <summary>
        /// 任务执行开始时间
        /// </summary>
        private static DateTime _workstarttime;



        /// <summary>
        /// 主线程任务
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("  ");
            Console.WriteLine("  分布式ID生成测试器启动...");
            Console.WriteLine("  分配线程任务...\r\n");

            //初始化 参数
            _thredsokcount = 0;
            _worksdata = new long[_thredscount * _thredworkcount];
            _workstarttime = DateTime.Now;

            //Console.WriteLine((long)(new DateTime(2018, 10, 25, 0, 0, 0, DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            //分配线程任务
            for (int i = 1; i <= _thredscount; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(TestFun), i.ToString());
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 子线程执行方法 创建ID
        /// </summary>
        /// <param name="obj"></param>
        public static void TestFun(object obj)
        {
            int thredid = Convert.ToInt32(obj);
            var _scoubnt = ((thredid - 1) * _thredworkcount);
            //子线程执行开始时间
            DateTime dt1 = DateTime.Now;

            Console.WriteLine(string.Format("  线程 {0} 任务执行中...", thredid));

            for (int i = 0; i < _thredworkcount; i++)
            {
                try
                {
                    var _id = _snowflake.nextId();
                    _worksdata[_scoubnt + i] = _id;
                    //Console.WriteLine(string.Format("  线程 {0} 生成ID：{1}   二进制：{2}", thredid, _id, Convert.ToString(_id, 2).PadLeft(64, '0')));

                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("  线程 {0} 生成ID异常：{1}", thredid, e.Message));
                    break;
                }

            }
            Console.WriteLine(string.Format("  线程 {0} 执行 {1} 次ID生成 任务完毕，耗时：{2} 秒 ",
                thredid, _thredworkcount, (DateTime.Now - dt1).TotalSeconds));

            _thredsokcount++;

            //任务执行完毕
            if (_thredsokcount == _thredscount)
            {

                Console.WriteLine(string.Format("  \r\n  所有线程执行完毕,总耗时：{0} 秒", (DateTime.Now - _workstarttime).TotalSeconds));
                Console.WriteLine(string.Format("  \r\n  开始比较生成ID数据的重复项..."));
                var result = Repeat(_worksdata);
                Console.WriteLine(string.Format("  生成 {0} 条ID 里面包含 {1} 条重复ID ", _thredscount * _thredworkcount, result.Count));
                foreach (var item in result)
                {
                    Console.WriteLine(string.Format("  重复ID：{0}", item));
                }
            }
        }

        /// <summary>
        /// 比较重复的项
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static List<long> Repeat(long[] array)
        {
            Hashtable ht = new Hashtable();
            List<long> reps = new List<long>();
            for (int i = 0; i < array.Length; i++)
            {
                if (ht.Contains(array[i]))
                {
                    reps.Add(array[i]);
                    continue;
                }
                else
                {
                    ht.Add(array[i], array[i]);
                }
            }
            return reps;
        }


    }
}
