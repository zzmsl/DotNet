using System;

using System.Web;
using System.Web.Caching;
using System.Collections;

namespace DotNet.Web
{
    public abstract class Cache
    {
        private static System.Web.Caching.Cache _cache;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private Cache() { }

        /// <summary>
        /// 第一次运行时 初始化 Cache
        /// </summary>
        static Cache()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = System.Web.HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// 清除 Cache 的所有项
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }
            foreach (string key in al)
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 清除对应值的 Cache
        /// </summary>
        public static void Clear(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// 写入 Cache
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="seconds">时间，秒数</param>
        /// <param name="level">级别，"normal"、"high"</param>
        public static void Insert(string key, object obj, int seconds, CachesLevel cl)
        {
            //if (level == "normal")
            //{
            //    _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
            //        CacheItemPriority.Normal, null);
            //}
            //else if(level == "high")
            //{
            //    _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
            //        CacheItemPriority.High, null);
            //}

            switch (cl)
            {
                case CachesLevel.Low:
                    {
                        _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration,
                            CacheItemPriority.Low, null);
                        break;
                    }
                case CachesLevel.Normal:
                    {
                        _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration,
                            CacheItemPriority.Normal, null);
                        break;
                    }
                case CachesLevel.High:
                    {
                        _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration,
                            CacheItemPriority.High, null);
                        break;
                    }
                default:
                    {
                        _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration,
                            CacheItemPriority.Normal, null);
                        break;
                    }
            }

        }

        /// <summary>
        /// 取得 key 对应的 Cache
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return _cache[key];
        }
        
    }

    /// <summary>
    /// Caches 的缓存级别
    /// </summary>
    public enum CachesLevel
    {
        /// <summary>
        /// 低
        /// </summary>
        Low,

        /// <summary>
        /// 中等
        /// </summary>
        Normal,

        /// <summary>
        /// 高
        /// </summary>
        High,
    }
}
