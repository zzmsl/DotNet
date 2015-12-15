using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Threading
{
    public static class Thread
    {
        /// <summary>
        /// 休眠，秒
        /// </summary>
        public static void Sleep(int second)
        {
            for (int i = 0; i < second; i++)
            {
                System.Threading.Thread.Sleep(1000);                
            }
        }

        /// <summary>
        /// 休眠，毫秒
        /// </summary>
        public static void SleepMillisecond(int millisecond)
        {
            System.Threading.Thread.Sleep(millisecond);
        }
    }
}
