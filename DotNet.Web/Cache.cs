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
        /// Ĭ�Ϲ��캯��
        /// </summary>
        private Cache() { }

        /// <summary>
        /// ��һ������ʱ ��ʼ�� Cache
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
        /// ��� Cache ��������
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
        /// �����Ӧֵ�� Cache
        /// </summary>
        public static void Clear(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// д�� Cache
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="obj">ֵ</param>
        /// <param name="seconds">ʱ�䣬����</param>
        /// <param name="level">����"normal"��"high"</param>
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
        /// ȡ�� key ��Ӧ�� Cache
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return _cache[key];
        }
        
    }

    /// <summary>
    /// Caches �Ļ��漶��
    /// </summary>
    public enum CachesLevel
    {
        /// <summary>
        /// ��
        /// </summary>
        Low,

        /// <summary>
        /// �е�
        /// </summary>
        Normal,

        /// <summary>
        /// ��
        /// </summary>
        High,
    }
}
