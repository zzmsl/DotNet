using System;
using System.Text;
using System.IO;

namespace DotNet.Net
{
    public class HttpWebResponse
    {
        public static System.Net.HttpWebResponse Get(string urlString, string referer, string accept,
            string userAgent, string cookie, bool allowAutoRedirect, int timeout)
        {
            System.Net.HttpWebRequest httpWebRequest
                = System.Net.WebRequest.Create(urlString) as System.Net.HttpWebRequest;

            httpWebRequest.Timeout = timeout * 1000;
            httpWebRequest.Method = "Get";

            if (!string.IsNullOrEmpty(cookie))
                httpWebRequest.Headers.Add("Cookie", cookie);

            httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
            httpWebRequest.Referer = referer;
            httpWebRequest.Accept = accept;
            httpWebRequest.UserAgent = userAgent;

            try
            {
                System.Net.HttpWebResponse httpWebResponse
                    = httpWebRequest.GetResponse() as System.Net.HttpWebResponse;
                return httpWebResponse;
            }
            catch
            { return null; }
        }

        public static string GetHtml(string urlString, string referer, string accept,
            string userAgent, string cookie, bool allowAutoRedirect, int timeout, string encodeName)
        {
            System.Net.WebResponse httpWebResponse = DotNet.Net.HttpWebResponse.Get(
                urlString, referer, accept, userAgent, cookie, allowAutoRedirect, timeout);
            string html = "";

            if (httpWebResponse != null)
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(
                    httpWebResponse.GetResponseStream()
                    , System.Text.Encoding.GetEncoding(encodeName));
                html = streamReader.ReadToEnd();
            }
            return html;
        }

        public static System.IO.Stream GetFile(string urlString, string referer, string accept,
            string userAgent, string cookie, bool allowAutoRedirect, int timeout, string encodeName)
        {
            System.Net.WebResponse httpWebResponse = DotNet.Net.HttpWebResponse.Get(
                urlString, referer, accept, userAgent, cookie, allowAutoRedirect, timeout);            
            Stream stream = null;
            if (httpWebResponse != null)
            {                
                stream = httpWebResponse.GetResponseStream();
            }
            return stream;
        }

        public static string GetGZipHtml(string urlString, string referer, string accept,
            string userAgent, string cookie, bool allowAutoRedirect, int timeout, string encodeName)
        {
            System.Net.WebResponse httpWebResponse = DotNet.Net.HttpWebResponse.Get(
                urlString, referer, accept, userAgent, cookie, allowAutoRedirect, timeout);
            string html = "";

            if (httpWebResponse != null)
            {
                Stream stream = httpWebResponse.GetResponseStream();
                //byte[] buffer = new byte[100];
                //int length = 0;
                //using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
                //{
                //    using (MemoryStream msTemp = new MemoryStream())
                //    {
                //        while ((length = gz.Read(buffer, 0, buffer.Length)) != 0)
                //        {
                //            msTemp.Write(buffer, 0, length);
                //        }

                //        html = System.Text.Encoding.GetEncoding("gb2312").GetString(msTemp.ToArray());
                //    }

                //}
                html = System.Text.Encoding.GetEncoding(encodeName).GetString(DotNet.Tools.Utility.GZipDecompress(stream));

            }
            return html;
        }

        /// <summary>
        /// Post结束后，获取返回的Html
        /// </summary>
        /// <param name="urlString"></param>
        /// <param name="postData"></param>
        /// <param name="referer"></param>
        /// <param name="accept"></param>
        /// <param name="userAgent"></param>
        /// <param name="cookie"></param>
        /// <param name="timeout"></param>
        /// <param name="encodeName"></param>
        /// <returns></returns>
        public static string GetPostHtml(string urlString, string postData, string referer, string accept,
            string userAgent, string cookie, int timeout, string encodeName)
        {
            System.Net.WebResponse httpWebResponse = DotNet.Net.HttpWebResponse.Post(
                urlString, postData, referer, accept, userAgent, cookie,  timeout, encodeName);
            string html = "";

            if (httpWebResponse != null)
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(
                    httpWebResponse.GetResponseStream()
                    , System.Text.Encoding.GetEncoding(encodeName));
                html = streamReader.ReadToEnd();
            }
            return html;
        }

        public static System.Net.HttpWebResponse Post(string urlString, string postData, string referer, string accept,
            string userAgent, string cookie, int timeout, string encodeName)
        {
            System.Net.HttpWebRequest httpWebRequest
                = System.Net.WebRequest.Create(urlString) as System.Net.HttpWebRequest;
            //httpWebRequest.Timeout = httpWebRequest_Timeout;
            httpWebRequest.Timeout = timeout * 1000;
            httpWebRequest.Method = "Post";

            if (!string.IsNullOrEmpty(cookie))
                httpWebRequest.Headers.Add("Cookie", cookie);

            //staticusername=AAA&staticpassword=BBB
            byte[] byteHttpWebRequest = Encoding.GetEncoding(encodeName).GetBytes(postData);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.Referer = referer;
            httpWebRequest.Accept = accept;
            httpWebRequest.UserAgent = userAgent;

            httpWebRequest.ContentLength = byteHttpWebRequest.Length;

            try
            {
                Stream postStream = httpWebRequest.GetRequestStream();
                postStream.Write(byteHttpWebRequest, 0, byteHttpWebRequest.Length);
                postStream.Close();

                System.Net.HttpWebResponse httpWebResponse
                    = httpWebRequest.GetResponse() as System.Net.HttpWebResponse;
                return httpWebResponse;
            }
            catch
            { return null; }
        }

        /******/

        public static string GetPostHtml(string urlString, string postData, string contentType, int timeout, string encodeName)
        {
            System.Net.WebResponse httpWebResponse = DotNet.Net.HttpWebResponse.Post(
                urlString, postData, contentType, timeout, encodeName);
            string html = "";

            if (httpWebResponse != null)
            {
                System.IO.StreamReader streamReader = new System.IO.StreamReader(
                    httpWebResponse.GetResponseStream()
                    , System.Text.Encoding.GetEncoding(encodeName));
                html = streamReader.ReadToEnd();
            }
            return html;
        }
        public static System.Net.HttpWebResponse Post(string urlString, string postData, string contentType, int timeout, string encodeName)
        {
            System.Net.HttpWebRequest httpWebRequest
                = System.Net.WebRequest.Create(urlString) as System.Net.HttpWebRequest;

            httpWebRequest.Timeout = timeout * 1000;
            httpWebRequest.Method = "Post";

            byte[] byteHttpWebRequest = Encoding.GetEncoding(encodeName).GetBytes(postData);

            httpWebRequest.ContentType = contentType;
            httpWebRequest.AllowAutoRedirect = false;

            httpWebRequest.ContentLength = byteHttpWebRequest.Length;

            try
            {
                Stream postStream = httpWebRequest.GetRequestStream();
                postStream.Write(byteHttpWebRequest, 0, byteHttpWebRequest.Length);
                postStream.Close();

                System.Net.HttpWebResponse httpWebResponse
                    = httpWebRequest.GetResponse() as System.Net.HttpWebResponse;
                return httpWebResponse;
            }
            catch
            { return null; }
        }

        public static System.Net.HttpWebResponse PostFile(string urlString, string filePath, string referer, string accept,
            string userAgent, string cookie, int timeout)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");//时间戳 
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

            StringBuilder sbPostHeader = new StringBuilder();
            sbPostHeader.Append("--");
            sbPostHeader.Append(strBoundary);
            sbPostHeader.Append("\r\n");
            sbPostHeader.Append("Content-Disposition: form-data; name=\"");
            sbPostHeader.Append("Filedata");
            sbPostHeader.Append("\"; filename=\"");
            sbPostHeader.Append(System.IO.Path.GetFileName(filePath));
            sbPostHeader.Append("\";");
            sbPostHeader.Append("\r\n");
            sbPostHeader.Append("Content-Type: ");
            sbPostHeader.Append("application/octet-stream");
            sbPostHeader.Append("\r\n");
            sbPostHeader.Append("\r\n");

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbPostHeader.ToString());
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;

            System.Net.HttpWebRequest httpWebRequest =
                System.Net.WebRequest.Create(urlString) as System.Net.HttpWebRequest;

            httpWebRequest.ContentLength = length;
            httpWebRequest.AllowWriteStreamBuffering = false;
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + strBoundary;

            httpWebRequest.Timeout = timeout * 1000;
            httpWebRequest.Method = "Post";

            if (!string.IsNullOrEmpty(cookie))
                httpWebRequest.Headers.Add("Cookie", cookie);

            httpWebRequest.Referer = referer;
            httpWebRequest.Accept = accept;
            httpWebRequest.UserAgent = userAgent;

            //每次上传4k
            int bufferLength = 4096;
            byte[] buffer = new byte[bufferLength];

            try
            {
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpWebRequest.GetRequestStream();
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);//发送请求头部消息 

                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    size = r.Read(buffer, 0, bufferLength);
                }

                //添加尾部的时间戳 
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();

                //获取服务器端的响应 
                System.Net.HttpWebResponse httpWebResponse
                    = httpWebRequest.GetResponse() as System.Net.HttpWebResponse;
                return httpWebResponse;

            }
            catch
            { return null; }
            finally
            { fs.Close(); r.Close(); }
        }


        /// <summary>
        /// 非断点续传
        /// </summary>
        public static bool FileDownload(string savePath, string urlString)
        {
            //AdPlayerGlobal.IsDownloading = true;
            //AdPlayerGlobal.DownloadIndex += 1;

            long length = 0;//本次需下载的大小
            long downloaded = 0;//本次累计下载
            int buff = 512;//buff
            int read = 0;//从流中读取的字节

            string fileName = Path.GetFileName(urlString);//返回abc.mp4，找不到返回空
            //string fileName = Common.UrlToFileName(urlString);
            string ext = Path.GetExtension(urlString);//返回.mp4，找不到返回空
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(ext)) return false;

            string sourcePath = savePath + fileName;
            savePath = sourcePath.Replace(ext, ".tmp");

            if (File.Exists(sourcePath)) return true;

            if (File.Exists(savePath)) File.Delete(savePath);//存在就先删除

            FileStream fs = new FileStream(savePath, System.IO.FileMode.Create);

            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(urlString);
                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;

                length = Convert.ToInt64(response.Headers.Get("Content-Length"));//文件总大小                
                if (length == 0) return false;//获取不到文件总大小，表示出错

                //开始下载                
                Stream stream = response.GetResponseStream();

                byte[] bytes = new byte[buff];
                read = stream.Read(bytes, 0, buff);

                while (read > 0)
                {
                    downloaded += read;

                    fs.Write(bytes, 0, read);
                    read = stream.Read(bytes, 0, buff);

                    //AdPlayerGlobal.DownloadPrecent = (Convert.ToDecimal(downloaded) / Convert.ToDecimal(length)) * 100;

                    //if (AdPlayerGlobal.IsCancel)
                    //{
                    //    AdPlayerGlobal.IsDownloading = false; fs.Close(); stream.Close(); return false;
                    //}
                }

                fs.Close(); stream.Close();
                //Common.ThreadSleep(1);

                string oldSavePath = savePath;
                string newSavePath = savePath.Replace(".tmp", ext);
                File.Move(oldSavePath, newSavePath);//下载完毕，改名为原文件名
            }
            catch
            {
                fs.Close();
                //AdPlayerGlobal.IsDownloading = false;
                return false;
            }

            //AdPlayerGlobal.IsDownloading = false;
            return true;
        }

        #region 断点续传，改过后具体没测试
        public static bool FileDownload(string savePath, string urlString, int fileLength)
        {
            long offset = 0;//本地已存在的文件大小
            long length = 0;//本次需下载的大小
            long downloaded = 0;//本次累计下载

            FileStream fs;

            if (File.Exists(savePath))
            {
                fs = File.OpenWrite(savePath);
                offset = fs.Length;
                fs.Seek(offset, SeekOrigin.Current); //移动文件流中的当前指针 
            }
            else
            {
                fs = new FileStream(savePath, System.IO.FileMode.Create);
                offset = 0;
            }

            //本地文件大于等于文件总大小，即是已经下载完毕
            if (offset >= fileLength) return true;

            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(urlString);
                if (offset > 0 && offset < int.MaxValue)
                    request.AddRange(Convert.ToInt32(offset)); //设置Range值，取从指定的部分开始至结束
                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;

                length = Convert.ToInt64(response.Headers.Get("Content-Length"));//总大小减去offset的值
                if (length == 0) return false;//获取不到文件总大小，表示出错

                //开始下载
                Stream stream = response.GetResponseStream();

                int buff = 512;
                byte[] bytes = new byte[buff];
                int read = 0;
                read = stream.Read(bytes, 0, buff);

                while (read > 0)
                {
                    downloaded += read;

                    fs.Write(bytes, 0, read);
                    read = stream.Read(bytes, 0, buff);

                    //AdPlayerGlobal.DownloadPrecent = (Convert.ToDecimal(downloaded) / Convert.ToDecimal(length));

                    //if (AdPlayerGlobal.IsCancel) return false;
                }

                fs.Close(); stream.Close();
            }
            catch
            { fs.Close(); return false; }

            return true;
        }
        #endregion
    }
}
