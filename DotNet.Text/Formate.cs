using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace DotNet.Text
{
    public class Formate
    {
        #region 字符截取，移除等

        /// <summary>
        /// 按大小截断字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SubString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            //只要从第0个字符开始的maxLength个字符
            if (input.Length > maxLength)
                input = input.Substring(0, maxLength);

            return input;

        }

        /// <summary>
        /// 返回 string.substring
        /// 从startIndex的下一个开始，共length个，string的length从1开始
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubString(string str, int startIndex, int length)
        {
            if (!string.IsNullOrEmpty(str) && str.Length >= startIndex + length)
            {
                return str.Substring(startIndex, length);
            }
            else return "";
        }

        /// <summary>
        /// 取得开头到下一个结束之间的内容，是否包括这个开头和结尾的字符
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="enableInclude"></param>
        /// <returns></returns>
        public static string GetBetweenValue(string inputString, string startValue, string endValue, bool enableInclude)
        {
            string temp = "";
            if (string.IsNullOrEmpty(inputString) || string.IsNullOrEmpty(startValue) || string.IsNullOrEmpty(endValue)) return temp;

            int n = inputString.IndexOf(startValue);
            if (n >= 0)
            {
                //去掉前面的
                if (!enableInclude)
                    inputString = inputString.Substring(n + startValue.Length, inputString.Length - n - startValue.Length);
                else if (enableInclude)
                    inputString = inputString.Substring(n, inputString.Length - n - startValue.Length);
            }

            int m = inputString.IndexOf(endValue);

            if (n >= 0 && m >= 0)
            {
                //去掉后面的
                if (!enableInclude) temp = inputString.Substring(0, m);
                else if (enableInclude)
                    temp = inputString.Substring(0, m + endValue.Length);
            }

            return temp;
        }

        /// <summary>
        /// 取得开头到下一个结束之间的所有内容，例如html中多个<a href="url">title</a> 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="enableInclude"></param>
        /// <returns></returns>
        public static List<string> GetBetweenValues(string inputString, string startValue, string endValue, bool enableInclude)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(inputString) || string.IsNullOrEmpty(startValue) || string.IsNullOrEmpty(endValue)) return list;
            int m = 0; int n = 0;

            while (inputString.IndexOf(startValue) >= 0)
            {
                n = inputString.IndexOf(startValue);

                if (enableInclude) inputString = inputString.Substring(n, inputString.Length - n);
                else if (!enableInclude)
                    inputString = inputString.Substring(n + startValue.Length, inputString.Length - n - startValue.Length);

                if (enableInclude) m = inputString.IndexOf(endValue) + endValue.Length;
                else if (!enableInclude)
                    m = inputString.IndexOf(endValue);

                string temp = inputString.Substring(0, m);
                inputString = inputString.Replace(temp, "");

                list.Add(temp);
            }
            return list;
        }

        /// <summary>
        /// 移除开头到下一个结束之间的所有内容，例如html中多个<!-- -->
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="enableInclude"></param>
        /// <returns></returns>
        public static string RemoveBetweenValue(string inputString, string startValue, string endValue)
        {
            int m = 0; int n = 0;

            do
            {
                m = inputString.IndexOf(startValue);
                if (m >= 0) n = inputString.IndexOf(endValue, m);
                if (m >= 0 && n >= 0 && n > m) inputString = inputString.Remove(m, n - m + endValue.Length);

                m = inputString.IndexOf(startValue);
                if (m >= 0) n = inputString.IndexOf(endValue, m);
            }
            while (m >= 0 && n >= 0 && n > m);

            return inputString;
        }

        #endregion

        #region 过滤输入，格式化等

        /// <summary>
        /// 使用正则表达式过滤输入
        /// 过滤中文，英文之外的字符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RegexFilterHtml(string input)
        {
            //第一个 ^ 表示除外，中文、英文、数字；也就是过滤中文，英文之外的字符。
            const string reStr = "[^\u4e00-\u9fa5a-zA-Z0-9]";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(reStr);
            return regex.Replace(input, "");
        }

        public static bool IsNumber(string str)
        {
            const string reStr = "[^0-9]";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(reStr);
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 格式化html代码，需要配合RemoveHtml()使用
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FormateHtml(string html, bool enableHtml, bool em)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            string[] temp = html.Split('\n');
            html = "";

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = RemoveHtml(temp[i]);
                if (temp[i].Length > 0)
                {
                    if (enableHtml && em)
                    {
                        html += "<p>　　" + temp[i] + "</p>\n";
                    }
                    else if (enableHtml && !em)
                    {
                        html += "<p>" + temp[i] + "</p>\n";
                    }
                    else
                    {
                        html += temp[i];
                    }
                }
            }
            return html;
        }

        /// <summary>
        /// 除去html代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            Regex regexHtml = new Regex(@"<[^>]+>", RegexOptions.Compiled | RegexOptions.IgnoreCase);// <>尖括号里面的内容            
            Regex regexSpace = new Regex("\\s{2,}|\\ \\;", RegexOptions.Compiled | RegexOptions.IgnoreCase);//空格等           

            html = regexHtml.Replace(html, string.Empty);
            html = regexSpace.Replace(html, " ");
            return html.Trim();
        }

        /// <summary>
        /// 过滤输入的字符串，并转换掉 html
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CleanString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            StringBuilder sb = new StringBuilder();
            string output = "";

            // 不为空开始处理
            if (!string.IsNullOrEmpty(input))
            {
                //只要从第0个字符开始的maxLength个字符
                if (input.Length > maxLength)
                    input = input.Substring(0, maxLength);

                sb = new StringBuilder(input);
                //过滤字符

                sb.Replace("\"", "&quot;");
                sb.Replace("<", "&lt;");
                sb.Replace(">", "&gt;");
                sb.Replace("'", " ");

                output = sb.ToString();

                //消除替换掉后，可能会造成超过 maxLength 个字符串的 bug
                if (output.Length > maxLength)
                    output = output.Substring(0, maxLength);
            }
            return output;
        }


        /// <summary>
        /// 输入字节数，返回最接近的数，例如1023G，1023M，1023KB等
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytes(long bytes)
        {
            if (bytes == 0) return "0";

            string[] units = { "B", "KB", "MB", "GB", "TB", "EB", "ZB", "YB" };
            double db = bytes;
            int level = 0;
            while (db > 1024)
            {
                db /= 1024;
                level++;
            }
            return db.ToString(".##") + units[level];
        }

        /// <summary>
        /// 输入秒数，返回x分x秒
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string FormatTime(int seconds)
        {
            if (seconds == 0) return "0";
            int min = seconds / 60;
            int sec = seconds % 60;
            if (sec != 0)
                return min.ToString() + "分" + sec.ToString() + "秒";
            else return min.ToString() + "分";
        }

        /// <summary>
        /// 格式化显示时间为几个月,几天前,几小时前,几分钟前,或几秒前
        /// </summary>
        /// <param name="dt">要格式化显示的时间</param>
        /// <returns>几个月,几天前,几小时前,几分钟前,或几秒前</returns>
        public static string DateFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 180)
            {
                return dt.ToShortDateString();
            }
            else if (span.TotalDays > 90)
            {
                return "3个月前";
            }
            else if (span.TotalDays > 60)
            {
                return "2个月前";
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }       

        #endregion
    }
}
