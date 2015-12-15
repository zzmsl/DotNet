/*
 * 
公匙
<RSAKeyValue>
<Modulus>syHBv44RMq8dyeEh3a5mKG+3PppPZyx54r81n8qPbuBOz+e5YHxkEj66Tka+rFfQGrpsfBIX641CyR9i90L7haiIfT9/2iSPqRL/J8jrvLdZq+UVdgALyjz1RsgjCv3cwW+qZxykL6Q1xmIcRhDnexQfCxdPanCGQIPYAhjidWc=</Modulus>
<Exponent>AQAB</Exponent>
</RSAKeyValue>
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Security
{

    public class License
    {
        #region RSAKeyValue
        private const string RSAKeyValue_Exponent = "AQAB";
        private const string RSAKeyValue_D = "GrnkoGHf6wH5nc87MEn+7/WLD4oSYsBeBxikNY0RTWemGprmK6EjhR1ayqb/d9M6N2KtIO3KJ9jRqBRIvb9+qzh8Q3F6nT77aBNHyMUSn7iXM69DCUG0+35wYloGzWKdagtHnP/4ZgbJ2mFFWtagDohzen4ygr5d4D7TKmdnUeE=";
        private const string RSAKeyValue_Modulus = "syHBv44RMq8dyeEh3a5mKG+3PppPZyx54r81n8qPbuBOz+e5YHxkEj66Tka+rFfQGrpsfBIX641CyR9i90L7haiIfT9/2iSPqRL/J8jrvLdZq+UVdgALyjz1RsgjCv3cwW+qZxykL6Q1xmIcRhDnexQfCxdPanCGQIPYAhjidWc=";
        #endregion

        /// <summary>
        /// 加密过程,其中d、n是RSACryptoServiceProvider生成的D、Modulus 
        /// EncryptProcess(textBox1.Text, RSAKeyValue_D, RSAKeyValue_Modulus);       
        /// </summary>
        /// <param name="source"></param>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public string EncryptProcess(string source, string d, string n)
        {
            byte[] N = Convert.FromBase64String(n);
            byte[] D = Convert.FromBase64String(d);
            BigInteger biN = new BigInteger(N);
            BigInteger biD = new BigInteger(D);
            return EncryptString(source, biD, biN);
        }

        /// <summary>
        /// 功能：用指定的私钥(n,d)加密指定字符串source 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public string EncryptString(string source, BigInteger d, BigInteger n)
        {
            //int len = source.Length;
            //int len1 = 0;
            //int blockLen = 0;
            //if ((len % 128) == 0)
            //    len1 = len / 128;
            //else
            //    len1 = len / 128 + 1;
            //string block = "";
            //string temp = "";
            //for (int i = 0; i < len1; i++)
            //{
            //    if (len >= 128)
            //        blockLen = 128;
            //    else
            //        blockLen = len;
            //    block = source.Substring(i * 128, blockLen);
            //    byte[] oText = System.Text.Encoding.Default.GetBytes(block);
            //    BigInteger biText = new BigInteger(oText);
            //    BigInteger biEnText = biText.modPow(d, n);
            //    string temp1 = biEnText.ToHexString();
            //    temp += temp1;
            //    len -= blockLen;
            //}
            //return temp;


            //以下是改进 http://www.cnblogs.com/hhh/archive/2011/06/03/2070692.html
            int len = source.Length;
            int len1 = 0;
            int blockLen = 0;
            if ((len % 128) == 0)
                len1 = len / 128;
            else
                len1 = len / 128 + 1;
            string block = "";
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < len1; i++)
            {
                if (len >= 128)
                    blockLen = 128;
                else
                    blockLen = len;
                block = source.Substring(i * 128, blockLen);
                byte[] oText = System.Text.Encoding.Default.GetBytes(block);
                BigInteger biText = new BigInteger(oText);
                BigInteger biEnText = biText.modPow(d, n);
                string temp = biEnText.ToHexString();
                result.Append(temp).Append("@");
                len -= blockLen;
            }
            return result.ToString().TrimEnd('@');
        }

        /// <summary>
        ///  解密过程,其中e、n是RSACryptoServiceProvider生成的Exponent、Modulus 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string DecryptProcess(string source, string e, string n)
        {
            byte[] N = Convert.FromBase64String(n);
            byte[] E = Convert.FromBase64String(e);
            BigInteger biN = new BigInteger(N);
            BigInteger biE = new BigInteger(E);
            return DecryptString(source, biE, biN);
        }

        /// <summary>
        /// 功能：用指定的公钥(n,e)解密指定字符串source 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string DecryptString(string source, BigInteger e, BigInteger n)
        {
            StringBuilder result = new StringBuilder();
            string[] strarr1 = source.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strarr1.Length; i++)
            {
                string block = strarr1[i];
                BigInteger biText = new BigInteger(block, 16);
                BigInteger biEnText = biText.modPow(e, n);
                string temp = System.Text.Encoding.Default.GetString(biEnText.getBytes());
                result.Append(temp);
            }
            return result.ToString();
        }

        public static bool IsLicense(string[] license, string urlHost)
        {
            bool flag = false;
            if (license.Length == 0) return false;

            //一堆绑定的网址，有一个对即可，因为每次访问只用到一个，如 www.xxx.com 和 192.168.1.1
            foreach (string lic in license)
            {
                if (!string.IsNullOrEmpty(lic))
                    if (DecryptProcess(lic, RSAKeyValue_Exponent, RSAKeyValue_Modulus).Equals(urlHost))
                        flag = true;
            }
            return flag;
        }
    }
}
