using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.IO;

namespace DotNet.Security
{
    public class RSAUtility
    {
        public static RSAParameters ConvertFromPublicKey(string pemPublicKeyPath)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(pemPublicKeyPath))
            {
                RSAParameters para = new RSAParameters();

                byte[] data = new byte[fs.Length];
                byte[] keyData = null;
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    keyData = GetPem("PUBLIC KEY", data);
                }
                try
                {
                    //RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res);
                    //return rsa;

                    //keyData = Convert.FromBase64String(pemFileConent);
                    if (keyData.Length < 162)
                    {
                        throw new ArgumentException("pem file content is incorrect.");
                    }
                    byte[] pemModulus = new byte[128];
                    byte[] pemPublicExponent = new byte[3];
                    Array.Copy(keyData, 29, pemModulus, 0, 128);
                    Array.Copy(keyData, 159, pemPublicExponent, 0, 3);

                    para.Modulus = pemModulus;
                    para.Exponent = pemPublicExponent;

                }
                catch
                {
                }
                return para;
            }
        }

        public static RSACryptoServiceProvider LoadCertificateFile(string pemPrivateKeyPath)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(pemPrivateKeyPath))
            {
                byte[] data = new byte[fs.Length];
                byte[] res = null;
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    res = GetPem("RSA PRIVATE KEY", data);
                }
                try
                {
                    RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res);
                    return rsa;
                }
                catch
                {
                }
                return null;
            }
        }

        private static byte[] GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);
            string header = String.Format("-----BEGIN {0}-----\\n", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);
            string base64 = pem.Substring(start, (end - start));
            return Convert.FromBase64String(base64);
        }
        
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------  
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading  
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)  
                    binr.ReadByte();    //advance 1 byte  
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes  
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number  
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                //------ all private key components are Integer sequences ----  
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----  
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)     //expect integer  
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte  
            else
                if (bt == 0x82)
                {
                    highbyte = binr.ReadByte(); // data size in next 2 bytes  
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    //count = BitConverter.ToInt32(modint, 0);
                    count = BitConverter.ToInt16(modint, 0);
                }
                else
                {
                    count = bt;     // we already have the data size  
                }

            while (binr.ReadByte() == 0x00)
            {   //remove high order zeros in data  
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);       //last ReadByte wasn't a removed zero, so back up a byte  
            return count;
        }

        ///// <summary>
        ///// RSA签名
        ///// SHA1WithRSA
        ///// RSAParameters paraPrivateKey = DotNet.Security.PemConverter.ConvertFromPemPrivateKey(PRIVATEKEY);
        ///// </summary>
        ///// <param name="pemFileConent">-----BEGIN RSA PRIVATE KEY-----与-----END RSA PRIVATE KEY-----之间的字符复制过来</param>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static byte[] RSASignData(string pemFileConent, string data)
        //{
        //    RSAParameters paraPrivateKey = ConvertFromPemPrivateKey(pemFileConent);

        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.ImportParameters(paraPrivateKey);

        //    byte[] byteData = Encoding.UTF8.GetBytes(data);
        //    //byte[] signData = rsa.SignData(byteData, "SHA1"));

        //    return rsa.SignData(byteData, new SHA1CryptoServiceProvider());

        //    //return System.Convert.ToBase64String(signData);
        //    //return DotNet.Tools.Convert.ByteArrayToHexString(signData);
        //}

        ///// <summary>
        ///// RSA验证签名
        ///// RSAParameters paraPublicKey = DotNet.Security.PemConverter.ConvertFromPemPublicKey(PUBLICKEY);
        ///// </summary>
        ///// <param name="paraPublicKey">-----BEGIN PUBLIC KEY-----与-----END PUBLIC KEY-----之间的字符复制过来</param>
        ///// <param name="data"></param>
        ///// <param name="signData"></param>
        ///// <returns></returns>
        //public static bool RSAVerifyData(string pemFileConent, string data, string signData)
        //{
        //    RSAParameters paraPublicKey = ConvertFromPemPublicKey(pemFileConent);

        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.ImportParameters(paraPublicKey);

        //    byte[] byteData = Encoding.UTF8.GetBytes(data);
        //    byte[] byteSignData = Encoding.UTF8.GetBytes(signData);
        //    //return rsa.VerifyData(byteData, "SHA1", byteSignData);

        //    return rsa.VerifyData(byteData, new SHA1CryptoServiceProvider(), byteSignData);            
        //}


        ////.NET使用OpenSSL生成的pem密钥文件【做电子商务的朋友可能需要】 
        ////http://www.cnblogs.com/think/archive/2009/09/09/ConvertPem2RSAParemeters.html

        ///// <summary>
        ///// 将pem格式公钥转换为RSAParameters
        ///// </summary>
        ///// <param name="PemFileConent">pem公钥内容</param>
        ///// <returns>转换得到的RSAParamenters</returns>
        //public static RSAParameters ConvertFromPemPublicKey(string pemFileConent)
        //{
        //    if (string.IsNullOrEmpty(pemFileConent))
        //    {
        //        throw new ArgumentNullException("PemFileConent", "This arg cann't be empty.");
        //    }
        //    pemFileConent = pemFileConent.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");
        //    byte[] keyData = Convert.FromBase64String(pemFileConent);
        //    if (keyData.Length < 162)
        //    {
        //        throw new ArgumentException("pem file content is incorrect.");
        //    }
        //    byte[] pemModulus = new byte[128];
        //    byte[] pemPublicExponent = new byte[3];
        //    Array.Copy(keyData, 29, pemModulus, 0, 128);
        //    Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
        //    RSAParameters para = new RSAParameters();
        //    para.Modulus = pemModulus;
        //    para.Exponent = pemPublicExponent;
        //    return para;
        //}

        ///// <summary>
        ///// 将pem格式私钥转换为RSAParameters
        ///// </summary>
        ///// <param name="PemFileConent">pem私钥内容</param>
        ///// <returns>转换得到的RSAParamenters</returns>
        //public static RSAParameters ConvertFromPemPrivateKey(string pemFileConent)
        //{
        //    if (string.IsNullOrEmpty(pemFileConent))
        //    {
        //        throw new ArgumentNullException("PemFileConent", "This arg cann't be empty.");
        //    }
        //    pemFileConent = pemFileConent.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "").Replace("\r", "").Replace(" ","");

        //    int bbbbb = pemFileConent.Length;
        //    byte[] keyData = Convert.FromBase64String(pemFileConent);
        //    if (keyData.Length < 609)
        //    {
        //        throw new ArgumentException("pem file content is incorrect.");
        //    }

        //    int index = 11;
        //    byte[] pemModulus = new byte[128];
        //    Array.Copy(keyData, index, pemModulus, 0, 128);

        //    index += 128;
        //    index += 2;//141
        //    byte[] pemPublicExponent = new byte[3];
        //    Array.Copy(keyData, index, pemPublicExponent, 0, 3);

        //    index += 3;
        //    index += 4;//148
        //    byte[] pemPrivateExponent = new byte[128];
        //    Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

        //    index += 128;
        //    index += ((int)keyData[index + 1] == 64 ? 2 : 3);//279
        //    byte[] pemPrime1 = new byte[64];
        //    Array.Copy(keyData, index, pemPrime1, 0, 64);

        //    index += 64;
        //    index += ((int)keyData[index + 1] == 64 ? 2 : 3);//346
        //    byte[] pemPrime2 = new byte[64];
        //    Array.Copy(keyData, index, pemPrime2, 0, 64);

        //    index += 64;
        //    index += ((int)keyData[index + 1] == 64 ? 2 : 3);//412/413
        //    byte[] pemExponent1 = new byte[64];
        //    Array.Copy(keyData, index, pemExponent1, 0, 64);

        //    index += 64;
        //    index += ((int)keyData[index + 1] == 64 ? 2 : 3);//479/480
        //    byte[] pemExponent2 = new byte[64];
        //    Array.Copy(keyData, index, pemExponent2, 0, 64);

        //    index += 64;
        //    index += ((int)keyData[index + 1] == 64 ? 2 : 3);//545/546
        //    byte[] pemCoefficient = new byte[64];
        //    Array.Copy(keyData, index, pemCoefficient, 0, 64);

        //    RSAParameters para = new RSAParameters();
        //    para.Modulus = pemModulus;
        //    para.Exponent = pemPublicExponent;
        //    para.D = pemPrivateExponent;
        //    para.P = pemPrime1;
        //    para.Q = pemPrime2;
        //    para.DP = pemExponent1;
        //    para.DQ = pemExponent2;
        //    para.InverseQ = pemCoefficient;
        //    return para;
        //}



        ///// <summary>
        ///// RSA签名工具类。
        ///// </summary>
        ////public class RSAUtil
        ////{

        //    public static byte[] RSASign(string data, string privateKeyPem)
        //    {
        //        RSACryptoServiceProvider rsaCsp = LoadCertificateFile(privateKeyPem);
        //        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        //        byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");
        //        //return Convert.ToBase64String(signatureBytes);

        //        return signatureBytes;
        //    }

        //    private static byte[] GetPem(string type, byte[] data)
        //    {
        //        string pem = Encoding.UTF8.GetString(data);
        //        string header = String.Format("-----BEGIN {0}-----\\n", type);
        //        string footer = String.Format("-----END {0}-----", type);
        //        int start = pem.IndexOf(header) + header.Length;
        //        int end = pem.IndexOf(footer, start);
        //        string base64 = pem.Substring(start, (end - start));
        //        return Convert.FromBase64String(base64);
        //    }

        //    private static RSACryptoServiceProvider LoadCertificateFile(string filename)
        //    {
        //        using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
        //        {
        //            byte[] data = new byte[fs.Length];
        //            byte[] res = null;
        //            fs.Read(data, 0, data.Length);
        //            if (data[0] != 0x30)
        //            {
        //                res = GetPem("RSA PRIVATE KEY", data);
        //            }
        //            try
        //            {
        //                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res);
        //                return rsa;
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //            return null;
        //        }
        //    }

        //    private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        //    {
        //        byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

        //        // --------- Set up stream to decode the asn.1 encoded RSA private key ------
        //        MemoryStream mem = new MemoryStream(privkey);
        //        BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
        //        byte bt = 0;
        //        ushort twobytes = 0;
        //        int elems = 0;
        //        try
        //        {
        //            twobytes = binr.ReadUInt16();
        //            if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
        //                binr.ReadByte();    //advance 1 byte
        //            else if (twobytes == 0x8230)
        //                binr.ReadInt16();    //advance 2 bytes
        //            else
        //                return null;

        //            twobytes = binr.ReadUInt16();
        //            if (twobytes != 0x0102) //version number
        //                return null;
        //            bt = binr.ReadByte();
        //            if (bt != 0x00)
        //                return null;


        //            //------ all private key components are Integer sequences ----
        //            elems = GetIntegerSize(binr);
        //            MODULUS = binr.ReadBytes(elems);

        //            elems = GetIntegerSize(binr);
        //            E = binr.ReadBytes(elems);

        //            elems = GetIntegerSize(binr);
        //            D = binr.ReadBytes(elems);

        //            elems = GetIntegerSize(binr);
        //            P = binr.ReadBytes(elems);

        //            elems = GetIntegerSize(binr);
        //            Q = binr.ReadBytes(elems);

        //            elems = GetIntegerSize(binr);
        //            DP = binr.ReadBytes(elems);

        //            elems = GetIntegerSize(binr);
        //            DQ = binr.ReadBytes(elems);

        //            elems = GetIntegerSize(binr);
        //            IQ = binr.ReadBytes(elems);


        //            // ------- create RSACryptoServiceProvider instance and initialize with public key -----
        //            CspParameters CspParameters = new CspParameters();
        //            CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
        //            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024, CspParameters);
        //            RSAParameters RSAparams = new RSAParameters();
        //            RSAparams.Modulus = MODULUS;
        //            RSAparams.Exponent = E;
        //            RSAparams.D = D;
        //            RSAparams.P = P;
        //            RSAparams.Q = Q;
        //            RSAparams.DP = DP;
        //            RSAparams.DQ = DQ;
        //            RSAparams.InverseQ = IQ;
        //            RSA.ImportParameters(RSAparams);
        //            return RSA;
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //        finally
        //        {
        //            binr.Close();
        //        }
        //    }

        //    private static int GetIntegerSize(BinaryReader binr)
        //    {
        //        byte bt = 0;
        //        byte lowbyte = 0x00;
        //        byte highbyte = 0x00;
        //        int count = 0;
        //        bt = binr.ReadByte();
        //        if (bt != 0x02)		//expect integer
        //            return 0;
        //        bt = binr.ReadByte();

        //        if (bt == 0x81)
        //            count = binr.ReadByte();	// data size in next byte
        //        else
        //            if (bt == 0x82)
        //            {
        //                highbyte = binr.ReadByte();	// data size in next 2 bytes
        //                lowbyte = binr.ReadByte();
        //                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
        //                count = BitConverter.ToInt32(modint, 0);
        //            }
        //            else
        //            {
        //                count = bt;		// we already have the data size
        //            }

        //        while (binr.ReadByte() == 0x00)
        //        {	//remove high order zeros in data
        //            count -= 1;
        //        }
        //        binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
        //        return count;
        //    }
        ////}
    }
}
