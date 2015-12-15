
namespace DotNet.Net
{
    public class IPAddress
    {
        public static System.Net.IPAddress GetLocalIPv4Address()
        {
            System.Net.IPAddress localIP = System.Net.IPAddress.Parse("127.0.0.1");

            //获取本机所有的IP地址列表
            System.Net.IPAddress[] list = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());

            foreach (System.Net.IPAddress item in list)
            {
                //判断是否是IPv4地址
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = item;
                    break;
                }
                else continue;
            }
            return localIP;
        }

        //Cache 名称
        private const string CACHE_KEY = "IP_{0}";

        //记录密码错误次数的时间，单位：秒，，10分钟
        private const int CACHE_ERROR_TIME = 600;

        //封 IP 错误次数，应比下面的大，错误 20 次密码封掉 IP
        private const int IP_BAN_COUNT = 20;

        //记录密码错误次数，错误多少次后显示验证码
        //private const int ERROR_COUNT = 3;

        public IPAddress() { }

        /// <summary>
        /// 取得被封的 IP，若有，返回 true
        /// </summary>
        /// <param name="ip">192.168.0.1</param>
        public static bool GetIPBan(string ip)
        {
            string cacheKey = string.Format(CACHE_KEY, ip);
            if (DotNet.Web.Cache.Get(cacheKey) != null)
            {
                if ((int)DotNet.Web.Cache.Get(cacheKey) == IP_BAN_COUNT)
                {
                    //throw new AppException("您的 IP 已经被封，请稍后再试。", AppExceptionType.IPBan);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 错误计数
        /// </summary>
        /// <param name="ip">192.168.0.1</param>
        public static void SetErrorCount(string ip)
        {
            string cacheKey = string.Format(CACHE_KEY, ip);
            if (DotNet.Web.Cache.Get(cacheKey) == null)
            {
                //第一次错误，由于 Page_Load 和 PostBack，第一次错误时，应将值设为 2
                DotNet.Web.Cache.Insert(cacheKey, 2, CACHE_ERROR_TIME, DotNet.Web.CachesLevel.Low);
            }
            else
            {
                //第二次以后的错误
                int val = (int)DotNet.Web.Cache.Get(cacheKey);
                val += 1;
                DotNet.Web.Cache.Insert(cacheKey, val, CACHE_ERROR_TIME, DotNet.Web.CachesLevel.Low);
            }
        }

        ///// <summary>
        ///// 是否显示验证码，该 IP 已经错误了足够次数 true，否则 false
        ///// </summary>
        ///// <param name="ip">192.168.0.1</param>
        ///// <returns>bool</returns>
        //public static bool ShowValidateCode(string ip)
        //{
        //    string cacheKey = string.Format(CACHE_KEY, ip);
        //    if (AppCache.Get(cacheKey) == null)
        //    {
        //        //没有错误
        //        return false;
        //    }
        //    else
        //    {
        //        //第二次以后的错误
        //        int val = (int)AppCache.Get(cacheKey);
        //        //如果 >= 足够次数
        //        if (val >= ERROR_COUNT)
        //            return true;
        //        else
        //            return false;
        //    }
        //}

        /// <summary>
        /// 取得客户端的 IP
        /// </summary>
        /// <param name="current">System.Web.HttpContext</param>
        /// <returns>string 192.168.0.1</returns>
        public static string GetCustomerIP(System.Web.HttpContext current)
        {
            string ipAddress = string.Empty;

            //不用代理 Request.ServerVariables["HTTP_VIA"] == null
            if (current.Request.ServerVariables["HTTP_VIA"] == null)
            {
                //返回真实 IP
                if (current.Request.ServerVariables["REMOTE_ADDR"] != null)
                    ipAddress = current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                //返回代理的 IP
                if (current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    ipAddress = current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }

            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = "127.0.0.1";

            return ipAddress;
        }
    }
}
