using System;

namespace DotNet.IO
{
    public class File
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        public static bool Delete(string path)
        {
            try
            {
                System.IO.File.Delete(path);//文件不存在，删除的时候不产生异常，所以不检查是否文件存在。
                return true;
            }
            catch
            { return false; }
        }

        public static bool IsFileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        ///// <summary>
        ///// 返回当前.exe文件所在的目录，x:\xxx\xxx
        ///// </summary>
        //public static string CurrentDirectory()
        //{
        //    return System.Environment.CurrentDirectory;
        //}        

        /// <summary>
        /// 是否合格的上传文件扩展名
        /// </summary>
        /// <param name="extension"> .jpg </param>
        /// <returns></returns>
        public static bool IsAllowExtension(string extension)
        {
            extension = extension.ToLower();
            if (extension.Equals(".jpg") || extension.Equals(".gif") || extension.Equals(".jpeg") || extension.Equals(".png"))
                return true;
            else return false;
        }

        /// <summary>
        /// 是否合格的上传文件格式，判断文件前两个字节
        /// 255216 jpg
        /// 7173 gif
        /// 13780 png
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool IsAllowFile(System.IO.Stream stream)
        {
            byte[] buff = new byte[2];
            stream.Read(buff, 0, 2);
            string twoByte = buff[0].ToString() + buff[1].ToString();

            if (twoByte.Equals("255216") || twoByte.Equals("7173") || twoByte.Equals("13780")) return true; else return false;
        }

        /// <summary>
        /// 复制文件，如果路径不存在，则创建
        /// </summary>
        /// <param name="sourceFileName">源路径</param>
        /// <param name="destFileName">新路径</param>
        public static void CopyFile(string sourceFileName, string destFileName)
        {
            if (!System.IO.File.Exists(sourceFileName)) return;//原文件不存在
            System.IO.File.Copy(sourceFileName, destFileName, true);
        }

        public static void WriteTxt(string path, string inputString)
        {
            //lock (lockFile)
            {
                System.IO.StreamWriter sw = null;
                try
                {
                    sw = System.IO.File.AppendText(path);
                    sw.WriteLine(inputString);
                }
                catch { }
                finally
                { sw.Flush(); sw.Close(); }
            }
        }

        public static void WriteBinary(System.IO.Stream stream, string path, string filename)
        {
            byte[] b = new byte[1024];
            int read = 0;

            try
            {
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                using (System.IO.FileStream fs = new System.IO.FileStream(path + filename, System.IO.FileMode.Create))
                {
                    while ((read = stream.Read(b, 0, b.Length)) > 0)
                    {
                        fs.Write(b, 0, read);
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 取得目录下所有文件
        /// </summary>
        /// <param name="path">源路径（不包括文件名）</param>
        public static System.IO.FileInfo[] GetFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            return new System.IO.DirectoryInfo(path).GetFiles();
        }
    }
}
