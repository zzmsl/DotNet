using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DotNet.Tools
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public class Convert
    {
        public static byte ToByte(string input)
        {            
            byte output = 0;
            byte.TryParse(input, out output);
            return output;
        }
        /// <summary>
        /// 验证是否是 Int16（short），是：返回此实例的的等效 short，否：返回 0
        /// Int16 值类型表示值介于 -32768 到 +32767 之间的有符号整数。
        /// SQL 中对应 smallint
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static short ToInt16(string input)
        {
            short output = 0;
            // 尝试转换成 short
            short.TryParse(input, out output);
            return output;
        }

        /// <summary>
        /// 验证是否是 Int32（int），是：返回此实例的等效 int，否：返回 0，
        /// Int32 值类型表示值介于 -2,147,483,648 到 +2,147,483,647 之间的有符号整数。
        /// SQL 中对应 int
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int ToInt32(string inputString)
        {
            int output = 0;
            // 尝试转换成 int
            int.TryParse(inputString, out output);
            return output;
        }

        public static long ToInt64(string inputString)
        {
            long output = 0;
            long.TryParse(inputString, out output);
            return output;
        }

        public static decimal ToDecimal(string inputString)
        {
            decimal output = 0;
            decimal.TryParse(inputString, out output);
            return output;
        }

        public static float ToSingle(string inputString)
        {
            //float 是 System.Single 类型的别名
            float output = 0;
            float.TryParse(inputString, out output);
            return output;
        }

        public static double ToDouble(string inputString)
        {
            //float 是 System.Single 类型的别名
            double output = 0;
            double.TryParse(inputString, out output);
            return output;
        }

        public static uint ToUInt32(string inputString)
        {
            uint output = 0;
            uint.TryParse(inputString, out output);
            return output;
        }

        public static ushort ToUInt16(string inputString)
        {
            ushort output = 0;
            ushort.TryParse(inputString, out output);
            return output;
        }

        public static ulong ToUInt64(string inputString)
        {
            ulong output = 0;
            ulong.TryParse(inputString, out output);
            return output;
        }

        public static bool ToBoolean(string inputString)
        {
            if (inputString == "1" || inputString == "true" || inputString == "True") return true;
            else if (inputString == "0") return false;
            else return false;
        }

        public static ulong ToUInt64(float inputString)
        {
            return Convert.ToUInt64(inputString);
        }

        /// <summary>
        /// 验证是否是 DateTime，是：返回次实例的等效 DateTime，否：返回 DateTime.MinValue;
        /// datetime     (4字节)   
        ///从   1753   年   1   月   1   日到   9999   年   12   月   31   日的日期和时间数据，精确度为百分之三秒
        ///（等于   3.33   毫秒或   0.00333   秒）。如下表所示，把值调整到   .000、.003、或   .007   秒的增量。
        ///smalldatetime   (2字节)   
        ///从   1900   年   1   月   1   日到   2079   年   6   月   6   日的日期和时间数据，
        ///精确到分钟。29.998   秒或更低的   smalldatetime   值向下舍入为最接近的分钟，29.999   秒或更高的
        ///smalldatetime   值向上舍入为最接近的分钟。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string input)
        {
            DateTime output = DateTime.MinValue;

            //尝试转换成 DateTime
            DateTime.TryParse(input, out output);

            //1753-1-1 12:00:00 
            //DateTime output = new DateTime(1753,1,1,12,0,0);

            if (output.Equals(DateTime.MinValue))
                output = new DateTime(1753, 1, 1, 12, 0, 0);

            return output;
        }

        /// <summary>
        /// 根据字符返回 GuID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Guid ToGuid(string input)
        {
            if (string.IsNullOrEmpty(input)) input = "00000000-0000-0000-0000-000000000000";
            
            try
            {
                Guid guid = new Guid(input);
                return guid;
            }
            catch
            {
                Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
                return guid;
            }
            
        }

        /// <summary>
        /// 将时间戳转换成时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dateTime = dtStart.Add(toNow);
            return dateTime;
        }

        /// <summary>
        /// 将指定的时间转化为时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int TimeToStamp(DateTime dateTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(dateTime.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return ToInt32(timeStamp);
        }

        /// <summary>
        /// 判断字符是否是数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[0-9]+$");
            return reg.IsMatch(input);
        }

        /// <summary>
        /// 判断字符是否是手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string input)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"1[3,5,8]\d{9}"); ;
            return reg.IsMatch(input);
        }

        /// <summary>
        /// 判断字符是否是英文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglish(string input)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]+$");
            return reg.IsMatch(input);
        }        

        /// <summary>
        /// byte 转换成 16进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] input)
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            char[] chars = new char[input.Length * 2];
            for (int i = 0; i < input.Length; i++)
            {
                int b = input[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }
        /// <summary>
        /// 16进 转换成 byte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string input)
        {
            input = input.Replace(" ", "");
            byte[] buffer = new byte[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
                buffer[i / 2] = (byte)System.Convert.ToByte(input.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// JSON字符串转成对象
        /// </summary>
        public static T JsonToObject<T>(string jsonString)
        {
            //using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            //{
            //    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            //}

            //系统的默认格式是这样的：
            //[{ "firstName": "Brett", "lastName":"McLaughlin", "email": "abc@def.com" }]
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    return (T)serializer.ReadObject(ms);
                }
            }
            catch
            { return default(T); }//其实就是 null
        }

        /// <summary>
        /// 对象转成JSON字符串
        /// </summary>
        public static string ObjectToJson(object jsonObject)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        ///// <summary>
        ///// 编码
        ///// </summary>
        ///// <param name="s"></param>
        ///// <param name="encode"></param>
        ///// <returns></returns>
        //public static string StringToHexString(string s, Encoding encode)
        //{
        //    byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组 
        //    string result = string.Empty;
        //    for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开 
        //    {
        //        string t = System.Convert.ToString(s[i], 16);
        //        result += @"\u" + t.PadLeft(4, '0');
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 解码
        ///// </summary>
        ///// <param name="hs"></param>
        ///// <param name="encode"></param>
        ///// <returns></returns>
        //public static string HexStringToString(string hs, Encoding encode)
        //{
        //    hs = hs.Replace(@"\u", "%");
        //    string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
        //    byte[] b = new byte[chars.Length];
        //    //逐个字符变为16进制字节数据 
        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        b[i] = System.Convert.ToByte(chars[i], 16);
        //    }
        //    //按照指定编码将字节数组变为字符串 
        //    return encode.GetString(b);
        //}

        //public static string ConvertStringTo16(string varstrOrigin)
        //{
        //    if (string.IsNullOrEmpty(varstrOrigin))
        //        return "";
        //    else
        //    {
        //        string strResult = "";
        //        byte[] bt = Encoding.Default.GetBytes(varstrOrigin);
        //        for (int i = 0; i < bt.Length; i++)
        //            strResult += bt[i].ToString("X");
        //        return strResult;
        //    }
        //}

        //public static string ReConvert16ToString(string varstrOrigin)
        //{
        //    string strResult = "";
        //    string[] strs = new string[varstrOrigin.Length / 2];
        //    for (int i = 0; i < strs.Length; i++)
        //        strs[i] = varstrOrigin.Substring(2 * i, 2);
        //    foreach (string item in strs)
        //    {
        //        int iToInt = int.Parse(item.ToString(), NumberStyles.AllowHexSpecifier);
        //        strResult += ((char)iToInt).ToString();
        //    }
        //    return strResult;
        //}

    }
}
