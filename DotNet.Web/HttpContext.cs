using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Web
{
    public class HttpContext
    {
        /// <summary>
        /// 返回 Server.MapPath 
        /// ServerMapPath("/") --> E:\temp\
        /// </summary>
        /// <param name="path">abc/</param>
        /// <returns>E:\temp\</returns>
        public static string ServerMapPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }
    }
}
