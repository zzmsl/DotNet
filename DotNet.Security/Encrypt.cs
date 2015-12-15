using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.IO;

namespace DotNet.Security
{
    public class Encrypt
    {
        /// <summary>
        /// MD5 加密算法的实现
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string MD5(string sourceString)
        {
            if (string.IsNullOrEmpty(sourceString))
                return "";

            const string HEX_TABLE = "0123456789ABCDEF";

            //byte[] hashData = Encoding.Default.GetBytes(str);
            byte[] hashData = Encoding.UTF8.GetBytes(sourceString);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            hashData = md5.ComputeHash(hashData);

            StringBuilder sb = new StringBuilder();
            sb.Length = hashData.Length * 2;

            for (int i = 0; i < hashData.Length; i++)
            {
                sb[i * 2] = HEX_TABLE[hashData[i] >> 4];
                sb[i * 2 + 1] = HEX_TABLE[hashData[i] & 0xF];
            }
            return sb.ToString();
        }

        /***/

        /// <summary>
        /// 加密字符，DecryptX为此加密的解密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string EncryptX(string sourceString, int x)
        {
            char[] sources = sourceString.ToCharArray();

            int length = sources.Length;
            char[] target = new char[length];

            for (int i = 0; i < length; i++)
            {
                int intChar = (int)sources[i] + x; // char-->int
                target[i] = (char)intChar; // int-->char
            }

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            s.Append(target);

            return s.ToString();
        }

        public static string DecryptX(string sourceString, int x)
        {
            char[] sources = sourceString.ToCharArray();

            int length = sources.Length;
            char[] target = new char[length];

            for (int i = 0; i < length; i++)
            {
                int intChar = (int)sources[i] - x; // char-->int
                target[i] = (char)intChar; // int-->char
            }

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            s.Append(target);

            return s.ToString();
        }
        /***/

        //public static string RSAEncrypt(string key, string data)
        //{
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.FromXmlString(key);

        //    byte[] byteData = Encoding.UTF8.GetBytes(data);
        //    byte[] encryptData = rsa.Encrypt(byteData, false);

        //    return Convert.ToBase64String(encryptData);            
        //}

        /// <summary>
        /// rsa加密 http://www.jackyinfo.com/post/2011/04/26/6480f.aspx
        /// 超过117字节的数据出错 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string key, string data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);

            byte[] byteData = Encoding.UTF8.GetBytes(data);
            byte[] encryptData = rsa.Encrypt(byteData, false);

            return System.Convert.ToBase64String(encryptData);
        }

        /// <summary>
        /// rsa解密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RSADecrypt(string key, string data)
        {
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //rsa.FromXmlString(key);

            //byte[] byteData = Encoding.UTF8.GetBytes(data);
            //byte[] encryptData = rsa.Encrypt(byteData, false);

            //return Convert.ToBase64String(encryptData);

//            const string key = @"
//            <RSAKeyValue>
//            <Modulus>zwyCboG/jIJZH5a2Ns3Iz0kKlJjGJBhid2kD69/S9SdzAmpb09vYUeOWiOhXlKmI01At0s5jQLGRdMH3zNcWkDf/jwqWvidVCGsiF48PBlrU9mDktQpVaEFECyZrzDAdcKLhvykiA+LbkSjgXjLqi7A6pzrr/8/qkaBuA639or0=</Modulus>
//            <Exponent>AQAB</Exponent>
//            <P>7b0A8AuQNxQ5cW4FHdF3CG5uGNl3W42RqmAo2lT7uR+aWnbOYw3t/Hsf8kXlAOyWKijuCg1cWhpCy8l8l01DNw==</P>
//            <Q>3vQDvcV3AXWTk99FEmEEX/OeMqsIKhqyK75qarwGYUOMaVJlaIbkWQIXXcf9WnNMXrp0tA/8lJ/SowUyQGSrqw==</Q>
//            <DP>IX1OdIIsfpXKwb/N2LV5HybvO3Dm726x5l5FYvw1uY5KIBQ8XpfHvplZlrdh2w9419eMML5RFCA+6JYphubLYQ==</DP>
//            <DQ>sCtuXWCP1YZbc2fjw08Hzu3IXk8I54QqrygKHIawH+DpLTvfR4X3H2HopsDlL7iVavC4UKOlOKdFIG11tK4Xew==</DQ>
//            <InverseQ>1fCcv2pl8sZz8ghaUpMFo6ZJrchxNBGx9NQwuIgtVQrj82agkN4QNIL4h2GCc6E1QLgB+zyyKZ+TFApHsVPrDg==</InverseQ>
//            <D>KgCgqShbIncC+3yIhH+koCByLAj+ES59MzozmHM0iZUyKKhI7qansJ0Z2bZApiDqZ+vZD+3chrA/EM+UpJJxufyc10bQuTaiNe+Hhw4acXmTxFcHyHOc6ElwNWybDWnw+7FtekBFN2s1W8a3lHX6ZCxd8L/c8E+4lUk/yGccvmU=</D>
//            </RSAKeyValue>";

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);

            byte[] byteData = System.Convert.FromBase64String(data);
            byte[] decryptData = rsa.Decrypt(byteData, false);

            return Encoding.UTF8.GetString(decryptData);
        }

        /// <summary>
        /// RSA签名，使用pem格式证书
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pemPrivateKeyPath"></param>
        /// <returns></returns>
        public static string RSASignData(string data, string pemPrivateKeyPath)
        {
            /*** OK ***/
            //RSACryptoServiceProvider rsa = RSAUtility.LoadCertificateFile(pemPrivateKeyPath);
            //byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            //byte[] signatureBytes = rsa.SignData(dataBytes, new SHA1CryptoServiceProvider());
            
            //return DotNet.Tools.Convert.ByteArrayToHexString(signatureBytes);
            /*** OK ***/

            string cacheKey = "Rsa_Pem_" + pemPrivateKeyPath;
            RSACryptoServiceProvider rsa = null;

            if (DotNet.Web.Cache.Get(cacheKey) != null)
                rsa = (RSACryptoServiceProvider)DotNet.Web.Cache.Get(cacheKey);
            else
            {
                rsa = RSAUtility.LoadCertificateFile(pemPrivateKeyPath);
                DotNet.Web.Cache.Insert(cacheKey, rsa, 600, DotNet.Web.CachesLevel.High);
            }
            
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = rsa.SignData(dataBytes, new SHA1CryptoServiceProvider());

            return DotNet.Tools.Convert.ByteArrayToHexString(signatureBytes);
        }

        /// <summary>
        /// 验证签名，使用公匙验证
        /// </summary>
        /// <param name="originalString">原文：UTF8编码</param>
        /// <param name="signatureString">签名：base64编码的字节</param>
        /// <param name="pemPublicKeyPath">公钥路径</param>
        /// <returns> 验签结果</returns>
        public static bool RSAVerifyData(string originalString, string signatureString, string pemPublicKeyPath)
        {
            /*** OK ***/
            //RSAParameters rsaPara = RSAUtility.ConvertFromPublicKey(pemPublicKeyPath);
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //rsa.ImportParameters(rsaPara);

            //byte[] dataBytes = Encoding.UTF8.GetBytes(originalString);
            //byte[] signatureBytes = DotNet.Tools.Convert.HexStringToByteArray(signatureString);

            //return rsa.VerifyData(dataBytes, new SHA1CryptoServiceProvider(), signatureBytes);
            /*** OK ***/

            string cacheKey = "Rsa_Pem_" + pemPublicKeyPath;
            RSACryptoServiceProvider rsa = null;

            if (DotNet.Web.Cache.Get(cacheKey) != null)
                rsa = (RSACryptoServiceProvider)DotNet.Web.Cache.Get(cacheKey);
            else
            {
                RSAParameters rsaPara = RSAUtility.ConvertFromPublicKey(pemPublicKeyPath);
                rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaPara);
                DotNet.Web.Cache.Insert(cacheKey, rsa, 600, DotNet.Web.CachesLevel.High);
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(originalString);
            byte[] signatureBytes = DotNet.Tools.Convert.HexStringToByteArray(signatureString);

            return rsa.VerifyData(dataBytes, new SHA1CryptoServiceProvider(), signatureBytes);
        }

        /// <summary>
        /// RSA签名，使用.Net生成的 RSAKeyValue
        /// 超过117字节的数据 不会 出错！
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RSASignDataXmlKey(string data, string key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = rsa.SignData(dataBytes, new SHA1CryptoServiceProvider());
            //return byteToHexStr(signatureBytes).ToString().ToLower();

            return DotNet.Tools.Convert.ByteArrayToHexString(signatureBytes);
        }

        /// <summary>
        /// RSA验证签名，使用.Net生成的 RSAKeyValue
        /// </summary>
        /// <param name="originalString"></param>
        /// <param name="signatureString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RSAVerifyDataXmlKey(string originalString, string signatureString, string key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);

            byte[] dataBytes = Encoding.UTF8.GetBytes(originalString);
            byte[] signatureBytes = DotNet.Tools.Convert.HexStringToByteArray(signatureString);

            return rsa.VerifyData(dataBytes, new SHA1CryptoServiceProvider(), signatureBytes);
        }

        /************************************/

        /// <summary>
        /// 创建DES密钥
        /// </summary>
        /// <returns></returns>
        public static string DESGenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="data">要加密的字符串。</param>
        /// <param name="key">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string DESEncrypt(string data, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(data);
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                /******/
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;       
         
                //StringBuilder ret = new StringBuilder();
                //foreach (byte b in ms.ToArray())
                //{
                //    ret.AppendFormat("{0:X2}", b);//{  index[,alignment][:formatString]} 
                //}
                //return ret.ToString();
                /******/
            }
        }

        /// <summary>
        /// 进行DES加密。可与Java版本对应
        /// </summary>
        /// <param name="data">要加密的字符串。</param>
        /// <param name="key">密钥，且必须为8位。</param>
        /// <param name="mode">Java一般写法：Cipher.getInstance("DES/ECB/PKCS5Padding");C# 这边要des.Mode = CipherMode.ECB;</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string DESEncrypt(string data, string key, CipherMode mode)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                //des.Mode = CipherMode.ECB;
                des.Mode = mode;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(data);
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                /******/
                //string str = Convert.ToBase64String(ms.ToArray());
                //ms.Close();
                //return str;

                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);//{  index[,alignment][:formatString]} 
                }
                return ret.ToString();
                /******/
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="data">要解密的以Base64</param>
        /// <param name="key">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string DESDecrypt(string data, string key)
        {
            /******/
            byte[] inputByteArray = Convert.FromBase64String(data);
            
            //byte[] inputByteArray = new byte[data.Length / 2];
            //for (int x = 0; x < data.Length / 2; x++)
            //{
            //    int i = (Convert.ToInt32(data.Substring(x * 2, 2), 16));
            //    inputByteArray[x] = (byte)i;
            //}
            /******/

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }        
    }
}
