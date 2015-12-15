using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.IO
{
    public class Path
    {
        /// <summary>
        /// 返回扩展名，如输入@"e:\wwwroot\bc.txt" 返回 .txt
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "";
            return System.IO.Path.GetExtension(fileName);
        }

        /// <summary>
        /// 返回文件名，如输入@"e:\wwwroot\bc.txt" 返回 bc.txt
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "";
            return System.IO.Path.GetFileName(fileName);
        }

        /// <summary>
        /// 返回目录名，如输入@"e:\wwwroot\bc.txt" 返回 e:\wwwroot
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "";
            return System.IO.Path.GetDirectoryName(fileName);
        }        
    }
}
