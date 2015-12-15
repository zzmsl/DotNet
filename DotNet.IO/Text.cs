using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.IO
{
    public class Text
    {
        private static object _lockFile = new object();        

        public static string ReadToString(string path)
        {
            if (!DotNet.IO.File.IsFileExists(path)) return "FileNotExists";//不存在直接返回

            System.IO.StreamReader sr = new System.IO.StreamReader(path, System.Text.Encoding.UTF8);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 读取文件，支持多线程
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<string> ReadToList(string path)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

            lock (_lockFile)
            {
                System.IO.StreamReader sr = null;
                try
                {
                    sr = new System.IO.StreamReader(path);
                    string strLine = sr.ReadLine();//读取文件中的一行            
                    while (strLine != null)//判断是否为空,表示到文件最后一行了
                    {
                        list.Add(strLine);
                        strLine = sr.ReadLine();
                    }
                }
                catch { }
                finally
                {
                    sr.Close();//关闭流
                }
            }
            return list;
        }

        /// <summary>
        /// 把日志写入文件，每日一个文件，支持多线程
        /// </summary>
        /// <param name="inputString"></param>
        public static void AppendText(string inputString, string path)
        {
            DateTime dateTimeNow = DateTime.Now;

            //20121125.txt
            //string path = _currentDirectory + @"\Log\" + dateTimeNow.Year + dateTimeNow.Month.ToString().PadLeft(2, '0') + dateTimeNow.Day.ToString().PadLeft(2, '0') + ".txt";

            lock (_lockFile)
            {
                System.IO.StreamWriter sw = null;
                try
                {
                    sw = System.IO.File.AppendText(path);
                    sw.WriteLine(dateTimeNow.ToString() + "  " + inputString);
                }
                catch { }
                finally
                {
                    sw.Flush(); sw.Close();
                }
            }
        }

        public static void WriteAllText(string inputString, string path)
        {
            //string path = _currentDirectory + @"\AdList.txt";
            try
            {
                System.IO.File.WriteAllText(path, inputString, Encoding.GetEncoding("UTF-8"));
            }
            catch { }
        }

        
    }
}
