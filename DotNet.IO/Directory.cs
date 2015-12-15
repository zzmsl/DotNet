using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.IO
{
    public class Directory
    {
        /// <summary>
        /// 按参数生（如 e:\temp\web）成目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        }

        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static System.IO.FileInfo[] GetFileInfo(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
                //throw new ArgumentException();

            return new System.IO.DirectoryInfo(path).GetFiles();
        }

        public static System.IO.DirectoryInfo[] GetDirectoryInfo(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
                //throw new ArgumentException();

            return new System.IO.DirectoryInfo(path).GetDirectories();
        }

        /// <summary>
        /// 返回当前.exe文件所在的目录，x:\xxx\xxx
        /// </summary>
        public static string CurrentDirectory()
        {
            return System.Environment.CurrentDirectory;
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
