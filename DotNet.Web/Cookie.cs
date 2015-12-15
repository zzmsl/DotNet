using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

namespace DotNet.Web
{
    public class Cookie
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="profileMod"></param>
        /// <param name="expires"></param>
        public static void WriteUserCookie(string userName, string password, int expires, string cookieName)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(cookieName)) return;

            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values["userName"] = userName;
            cookie.Values["token"] = password;//2014-7-26 fix DotNet.Text.Formate.SubString(password, 4, 8);//从第四个开始，共8个           

            if (expires > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expires);

            System.Web.HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void WriteUserCookieTicket(string token, int expires, string cookieName)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(cookieName)) return;

            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values["token"] = token;//2014-7-26 fix DotNet.Text.Formate.SubString(password, 4, 8);//从第四个开始，共8个           

            if (expires > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expires);

            System.Web.HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 取得用户登录的cookie信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetUserCookie(string cookieName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies[cookieName] != null
                && System.Web.HttpContext.Current.Request.Cookies[cookieName]["userName"] != null && System.Web.HttpContext.Current.Request.Cookies[cookieName]["token"] != null)
            {
                string key = System.Web.HttpContext.Current.Request.Cookies[cookieName]["userName"];
                string value = System.Web.HttpContext.Current.Request.Cookies[cookieName]["token"];
                dic.Add(key, value);
            }
            return dic;
        }

        public static string GetUserCookieTicket(string cookieName)
        {            
            string value = "";
            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies[cookieName] != null
                && System.Web.HttpContext.Current.Request.Cookies[cookieName]["token"] != null)
            {                
                value = System.Web.HttpContext.Current.Request.Cookies[cookieName]["token"];                
            }
            return value;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public static void ClearUserCookie(string cookieName)
        {
            //HttpCookie cookie = new HttpCookie("Cmcc");
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Clear();
            cookie.Expires = DateTime.Now.AddYears(-1);
            System.Web.HttpContext.Current.Response.AppendCookie(cookie);
        }
    }
}
