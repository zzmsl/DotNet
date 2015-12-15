using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace DotNet.Tools
{
    public class Utility
    {
        /// <summary>
        /// 返回指定范围的随机数
        /// </summary>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            //Random r = new Random();
            //return r.Next(10000).ToString();
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }

        /// <summary>
        /// 返回一定范围的随机数，小于300用
        /// </summary>
        /// <param name="total">从1至total之间</param>
        /// <param name="size">返回多少个</param>
        /// <returns></returns>
        public static List<int> RandomNumbers(int total, int size)
        {
            List<int> list = new List<int>();

            //产生total个数组
            int[] temp = new int[total];
            for (int i = 0; i < total; i++) temp[i] = i + 1;

            Random random = new Random();
            int end = total - 1;

            for (int i = 0; i < size; i++)
            {
                int num = random.Next(0, end + 1);
                list.Add(temp[num]);

                //打乱顺序,难点
                temp[num] = temp[end];
                end--;
            }
            
            return list;
        }

        /// <summary>
        /// 返回一定范围的随机数，大于300用
        /// </summary>
        /// <param name="total">从1至total之间</param>
        /// <param name="size">返回多少个</param>
        /// <returns></returns>
        public static List<int> RandomNumbers1(int total, int size)
        {
            List<int> list = new List<int>();
            int min = 0;
            int max = 0;
            for (int i = 1; i < size + 1; i++)
            {
                int t = total / size;
                min = t * (i - 1) + 1;
                max = t * i + 1;
                Random r = new Random(i * (int)DateTime.Now.Ticks);
                list.Add(r.Next(min, max));
            }
            return list;
        }

        /// <summary>
        /// 过滤数组中的重复内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] ArrayFilterRepeat(string[] input)
        {
            if (input.Length <= 1) return input;

            string temp = string.Empty;

            //从头和后比较
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = i + 1; j < input.Length; j++)
                {
                    //重复，清空之
                    if (input[i] == input[j])
                    {
                        input[i] = string.Empty;
                        continue;
                    }
                }
                //不为空才要
                if (input[i] != string.Empty)
                    temp += input[i] + ",";
            }

            //if (temp.LastIndexOf(",") > -1)
            if (temp.EndsWith(",")) temp = temp.Substring(0, temp.Length - 1);

            return temp.Split(',');
        }

        /// <summary>
        /// DateTime(1753, 1, 1, 12, 0, 0);
        /// </summary>
        /// <returns></returns>
        public static DateTime DateTimeMin()
        {
            return new DateTime(1753, 1, 1, 12, 0, 0);
        }

        /// <summary>
        /// 字符翻转
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string Reverse(string inputString)
        {            
            char[] charArray = inputString.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// 返回20130606
        /// </summary>
        /// <returns></returns>
        public static string YearMonthDateToString()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 返回130606
        /// </summary>
        /// <returns></returns>
        public static string YearMonthDateToString1()
        {
            return DateTime.Now.ToString("yyMMdd");
        }

        /// <summary>
        /// 当前时间的年，2013
        /// </summary>
        /// <returns></returns>
        public static string YearToString()
        {
            return DateTime.Now.ToString("yyyy");
        }

        /// <summary>
        /// 当前时间的月日，06
        /// </summary>
        /// <returns></returns>
        public static string MonthToString()
        {
            return DateTime.Now.ToString("MM");
        }

        /// <summary>
        /// 当前时间的日期，28
        /// </summary>
        /// <returns></returns>
        public static string DateToString()
        {
            return DateTime.Now.ToString("dd");
        }

        /// <summary>
        /// 返回095518
        /// </summary>
        /// <returns></returns>
        public static string TimeToString()
        {
            return DateTime.Now.ToString("HHmmss");
        }

        /// <summary>
        /// 返回095518123
        /// </summary>
        /// <returns></returns>
        public static string TimeToString1()
        {
            return DateTime.Now.ToString("HHmmssfff");
        }

        /// <summary>
        /// 返回 Server.MapPath 
        /// Server.MapPath("/") --> E:\temp\
        /// </summary>
        /// <param name="path">abc/</param>
        /// <returns>E:\temp\</returns>
        public static string ServerMapPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// 根据生日得出岁数
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public static int BirthdayAge(DateTime birthday)
        {
            if (birthday == DateTime.MinValue) return 0;
            TimeSpan ts = DateTime.Now - birthday;
            return new DateTime(ts.Ticks).Year;
        }        

        /// <summary>
        /// 交换两个int的值
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static int[] IntSwitch(int i, int j)
        {
            int k = 0;
            k = j; j = i; i = k;
            return new int[] { i, j };
        }

        /// <summary>
        /// 解压缩 用GZip压缩的流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] GZipDecompress(System.IO.Stream stream)
        {
            byte[] buffer = new byte[100];
            int length = 0;
            try
            {
                using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        while ((length = gz.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            ms.Write(buffer, 0, length);
                        }
                        return ms.ToArray();
                    }
                }
            }
            catch
            {
                return new byte[1];
            }
        }

        /// <summary>
        /// 根据输入分解后返回索引的内容
        /// </summary>
        /// <param name="inputString">a_b_c</param>
        /// <param name="separator">_</param>
        /// <param name="index">2</param>
        /// <returns>c</returns>
        public static string ArrayIndex(string inputString, char separator, int index)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;

            string[] array = inputString.Split(separator);

            if (array.Length > index)
                return array[index];
            else return string.Empty;
        }

        public static List<string> StringToList(string inputString, char separator)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(inputString)) return list;

            string[] array = inputString.Split(separator);
            foreach (string t in array)
            {
                if(!string.IsNullOrEmpty(t))
                    list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// http://localhost:1000/product/productprint.aspx?cc=yulin&id=123&un=
        /// 获取url指定参数的值，如上url，输入cc，获取yulin
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string UrlParam(string url, string paramName)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            int i = url.IndexOf('?') + 1;
            url = url.Substring(i, url.Length - i);

            string[] urlParams = url.Split('&');

            foreach (string urlParam in urlParams)
            {
                string[] t = urlParam.Split('=');
                if (t.Length == 2)
                {
                    if (t[0] == paramName)
                        return t[1];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成网站的目录名：20100707
        /// </summary>
        /// <returns></returns>
        public static string MakePathName()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 生成文件名 09531788888
        /// </summary>
        /// <returns></returns>
        public static string MakeFileName()
        {
            Random r = new Random();
            return DateTime.Now.ToString("HHmmss") + r.Next(10000).ToString();
        }

        /// <summary>
        /// 生成文件名 09531788888.jpg
        /// </summary>
        /// <returns></returns>
        public static string MakeFileName(string extName)
        {
            if (string.IsNullOrEmpty(extName)) return "";
            Random r = new Random();
            return DateTime.Now.ToString("HHmmss") + r.Next(10000).ToString() + extName;
        }

        public static Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlEncode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                sb.Append(temp.Key + "=" + System.Web.HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最后一个&字符
            int nLen = sb.Length;
            sb.Remove(nLen - 1, 1);

            return sb.ToString();
        }
    }
}
