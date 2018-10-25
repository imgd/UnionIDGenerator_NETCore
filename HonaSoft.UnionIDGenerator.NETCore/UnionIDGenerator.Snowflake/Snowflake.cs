using System;


/// <summary>
/// snowflake 是 twitter 开源的分布式ID生成算法
/// 其核心思想为，一个long型的ID:
/// 41 bit 作为毫秒数 - 41位的长度可以使用69年
/// 8 bit 数据中心ID 支持 255个数据中心
/// 5 bit 机器ID 支持 31个机器ID
/// 9 bit 作为毫秒内序列号 9位毫秒内产生511个ID序号  每秒支持511*1000=51.1万 ID
/// 最后还有一个符号位，永远是0
/// 
/// snowflake-64bit 示意图：
/// ###########################################################################################################
/// #                                                                                                         #
/// #             0  -  00000000000000000000000000000000000000000   -  00000000   - 00000    -  000000000     #
/// #             ↓                          ↓                            ↓           ↓             ↓         #
/// #        1位符号位                  41位时间戳                   8位数据中心    5位机器id     9位序列号       #
/// #                                                                                                         #
/// ###########################################################################################################
/// </summary>
/// 

namespace UnionIDGenerator.Snowflake
{

    public class Snowflake_Worker
    {
        /// <summary>
        /// 工作机器ID
        /// </summary>
        private static long workerId;

        /// <summary>
        /// 数据中心ID
        /// </summary>
        private static long datacenterId;

        /// <summary>
        /// 毫秒内序列 
        /// </summary>
        private static long sequence = 0L;

        /// <summary>
        /// 上次生成ID的时间截
        /// </summary>
        private static long lastTimestamp = -1L;


        /// <summary>
        /// 系统开始时间截 (UTC 2017-06-28 00:00:00)
        /// </summary>
        private static long twepoch = 1540425600000L;
        //private static long twepoch= (long)(new DateTime(2018, 10, 25, 0, 0, 0, DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        /// <summary>
        /// 机器id所占的位数
        /// </summary>
        private static long workerIdBits = 5L;

        /// <summary>
        /// 数据标识id所占的位数
        /// </summary>
        private static long datacenterIdBits = 8L;

        /// <summary>
        /// 支持的最大机器id(十进制)
        /// </summary>
        private static long maxWorkerId = -1L ^ (-1L << (int)workerIdBits);

        /// <summary>
        /// 支持的最大数据标识id
        /// </summary>
        private static long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits);

        /// <summary>
        /// 序列在id中占的位数
        /// </summary>
        private static long sequenceBits = 9L;

        /// <summary>
        /// 机器ID 左移位数
        /// </summary>
        private static long workerIdShift = sequenceBits;

        /// <summary>
        /// 数据标识id 左移位数
        /// </summary>
        private static long datacenterIdShift = sequenceBits + workerIdBits;

        /// <summary>
        /// 时间截向 左移位数 
        /// </summary>
        private static long timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;

        /// <summary>
        /// 生成序列的掩码
        /// </summary>
        private static long sequenceMask = -1L ^ (-1L << (int)sequenceBits);



        private static object syncRoot = new object();

        private static Snowflake_Worker _Snowflake_Worker = null;
        private Snowflake_Worker() { }
        public static Snowflake_Worker Instance(long _workerId, long _datacenterId)
        {
            if (_Snowflake_Worker == null)
            {
                _Snowflake_Worker = new Snowflake_Worker();
            }

            // sanity check for workerId
            if (_workerId > maxWorkerId || _workerId < 0)
            {
                throw new ArgumentException(string.Format("机器ID 不能大于  %d 或 小于 0", maxWorkerId));
            }
            if (_datacenterId > maxDatacenterId || _datacenterId < 0)
            {
                throw new ArgumentException(string.Format("数据中心ID 不能大于 %d 或 小于 0", maxDatacenterId));
            }

            workerId = _workerId;
            datacenterId = _datacenterId;

            return _Snowflake_Worker;
        }


        /// <summary>
        /// 线程安全的获得下一个 ID 的方法
        /// </summary>
        /// <returns></returns>
        public long nextId()
        {
            lock (syncRoot)
            {
                long timestamp = timeGen();

                //如果当前时间小于上一次ID生成的时间戳: 说明系统时钟回退过 - 这个时候应当抛出异常
                if (timestamp < lastTimestamp)
                {
                    throw new ApplicationException(string.Format("Clock moved backwards.  Refusing to generate id for %d milliseconds", lastTimestamp - timestamp));
                }

                //如果是同一时间生成的，则进行毫秒内序列
                if (lastTimestamp == timestamp)
                {
                    sequence = (sequence + 1) & sequenceMask;
                    //毫秒内序列溢出 即 序列 > 序列最大值
                    if (sequence == 0)
                    {
                        //阻塞到下一个毫秒,获得新的时间戳
                        timestamp = tilNextMillis(lastTimestamp);
                    }
                }
                //时间戳改变，毫秒内序列重置
                else
                {
                    sequence = 0L;
                }

                //上次生成ID的时间截
                lastTimestamp = timestamp;
                //移位并通过或运算拼到一起组成64位的ID
                return ((timestamp - twepoch) << (int)timestampLeftShift) | (datacenterId << (int)datacenterIdShift) | (workerId << (int)workerIdShift) | sequence;
            }
        }

        // 阻塞到下一个毫秒 即 直到获得新的时间戳
        protected long tilNextMillis(long lastTimestamp)
        {
            long timestamp = timeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = timeGen();
            }
            return timestamp;
        }

        // 获得以毫秒为单位的当前时间
        protected long timeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
