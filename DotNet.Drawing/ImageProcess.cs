using System;
using System.Collections.Generic;

using System.Text;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DotNet.Drawing
{
    public class ImageProcess
    {
        /// <summary>
        /// 获取图片的部分内容（已重载）
        /// </summary>
        /// <param name="largeImageUrl">大图地址</param>
        /// <param name="smallImageUrl">小图地址</param>
        /// <param name="width">选择框宽度</param>
        /// <param name="height">选择框高度</param>
        /// <param name="top">选择框距离图片上边框的距离高度</param>
        /// <param name="left">选择框距离图片左边框的距离宽度</param>
        public static void ImagePart(string sourceImageUrl, string destlImageUrl, int width, int height, int top, int left)
        {
            //if (File.Exists(largeImageUrl) || File.Exists(smallImageUrl)) return;            

            Image image = Image.FromFile(sourceImageUrl);
            Bitmap bm = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bm);

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle destRect = new Rectangle(new Point(0, 0), new Size(width, height));//新图大小（选中的框的大小）
            Rectangle origRect = new Rectangle(new Point(left, top), new Size(width, height));//原图位置（选中的框内的那块）

            //设置文字水印  
            //System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(b);
            //System.Drawing.Font font = new Font("Lucida Grande", 6);
            //System.Drawing.Brush brush = new SolidBrush(Color.Gray);
            //G.Clear(Color.White);
            g.DrawImage(image, destRect, origRect, GraphicsUnit.Pixel);

            //G.DrawString("www.yldt.com", font, brush, 0, 0);
            //G.Dispose();

            ImageCodecInfo ici = GetImageEncoder("JPEG");
            if (ici != null)
                bm.Save(destlImageUrl, ici, GetEncoderParameters(90));
            else
                bm.Save(destlImageUrl, ImageFormat.Jpeg);

            //b.Save(destlImageUrl, ImageFormat.Jpeg);
            image.Dispose(); bm.Dispose(); g.Dispose();
        }

        /// <summary>
        /// 获取图片的部分内容（已重载）
        /// </summary>
        /// <param name="largeImageUrl">大图地址</param>
        /// <param name="smallImageUrl">小图地址</param>
        /// <param name="width">选择框宽度</param>
        /// <param name="height">选择框高度</param>
        /// <param name="top">选择框距离图片上边框的距离高度</param>
        /// <param name="left">选择框距离图片左边框的距离宽度</param>
        /// <param name="origWidth">原图宽度</param>
        /// <param name="origHeight">原图高度</param>
        /// <param name="zoomWidth">缩放后图片宽度</param>
        /// <param name="zoomHeight">缩放后图片高度</param>
        public static void ImagePart(string sourceImageUrl, string destlImageUrl, int width, int height, int top, int left, int origWidth, int origHeight, int zoomWidth, int zoomHeight)
        {
            //没有缩放
            if (origWidth == zoomWidth && origHeight == zoomHeight)
            { ImagePart(sourceImageUrl, destlImageUrl, width, height, top, left); return; }

            //第一次画图，先画出缩放后的图片
            Image img1 = Image.FromFile(sourceImageUrl);
            Bitmap bm1 = new Bitmap(zoomWidth, zoomHeight);
            Graphics g1 = Graphics.FromImage(bm1);
            Rectangle destR1 = new Rectangle(0, 0, zoomWidth, zoomHeight);
            Rectangle origR1 = new Rectangle(0, 0, img1.Width, img1.Height);
            g1.CompositingQuality = CompositingQuality.HighQuality;
            g1.SmoothingMode = SmoothingMode.HighQuality;
            g1.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g1.DrawImage(img1, destR1, origR1, GraphicsUnit.Pixel);

            //第二次画图，在第一次画出的图上，截取相关部分
            Image img2 = Image.FromHbitmap(bm1.GetHbitmap());
            Bitmap bm2 = new Bitmap(width, height);
            Graphics g2 = Graphics.FromImage(bm2);
            Rectangle destR2 = new Rectangle(new Point(0, 0), new Size(width, height));//新图大小（选中的框的大小）
            Rectangle origR2 = new Rectangle(new Point(left, top), new Size(width, height));//原图位置（选中的框内的那块）
            g2.CompositingQuality = CompositingQuality.HighQuality;
            g2.SmoothingMode = SmoothingMode.HighQuality;
            g2.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g2.DrawImage(img2, destR2, origR2, GraphicsUnit.Pixel);

            ImageCodecInfo ici = GetImageEncoder("JPEG");
            if (ici != null)
                bm2.Save(destlImageUrl, ici, GetEncoderParameters(90));
            else
                bm2.Save(destlImageUrl, ImageFormat.Jpeg);
            //保存
            //bm2.Save(destlImageUrl, ImageFormat.Jpeg);

            //img1.Dispose(); bm1.Dispose(); g1.Dispose(); img2.Dispose(); bm2.Dispose(); g2.Dispose();

            //注意，释放也是有顺序的
            bm2.Dispose(); g2.Dispose(); img2.Dispose(); bm1.Dispose(); g1.Dispose(); img1.Dispose();
        }

        /// <summary>
        /// 获得图像的 width height
        /// [0]width
        /// [1]height
        /// </summary>
        /// <param name="sourceImageUrl"></param>
        /// <returns></returns>
        public static int[] GetImageWidthHeight(string sourceImageUrl)
        {
            Image img = Image.FromFile(sourceImageUrl);
            int width = img.Width;
            int height = img.Height;
            img.Dispose();//注意要disponse，要不无法释放
            return new int[2] { width, height };
        }

        /// <summary>
        /// 给图片增加水印文字，图片体积减少成倍，但质量会有所缺损，但是要很仔细才看得出。
        /// http://www.cnblogs.com/wdxinren/archive/2005/07/28/202230.html
        /// </summary>
        /// <param name="stream">上传的图片流</param>
        /// <param name="path">保存到的路径</param>
        /// <param name="waterMarkUrl">水印地址</param>
        /// <param name="corner">水印角落</param>
        public static void AddWatermarkToImg(System.IO.Stream stream, string destImageUrl, string waterMarkUrl, string corner)
        {
            //从上传图片的流 Stream 生成 Image
            Image imgUploadImage = Image.FromStream(stream);
            int imgUploadImageWidth = imgUploadImage.Width;
            int imgUploadImageHeight = imgUploadImage.Height;

            //生成上传图片的 Bitmap
            Bitmap bmUploadImage = new Bitmap(imgUploadImageWidth, imgUploadImageHeight);

            //从 水印图片生成 Image
            Image imgWaterMark = new Bitmap(waterMarkUrl);
            int wmWidth = imgWaterMark.Width;
            int wmHeight = imgWaterMark.Height;

            //生成水印的 Bitmap
            Bitmap bmWaterMark = new Bitmap(waterMarkUrl);

            //创建水印画布
            Graphics grWatermark = Graphics.FromImage(imgUploadImage);

            // 设置画布的描绘质量
            grWatermark.CompositingQuality = CompositingQuality.HighQuality;
            grWatermark.SmoothingMode = SmoothingMode.HighQuality;
            grWatermark.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int xPoint = 0;
            int yPoint = 0;

            switch (corner)
            {
                case "rightBottom"://右下角
                    xPoint = imgUploadImageWidth - wmWidth - 10;
                    yPoint = imgUploadImageHeight - wmHeight - 10;
                    break;
                case "rightTop"://右上角
                    xPoint = imgUploadImageWidth - wmWidth - 10;
                    yPoint = 10;
                    break;
                case "leftTop"://左上角
                    xPoint = 10;
                    yPoint = 10;
                    break;
                case "leftBottom"://左下角
                    xPoint = 10;
                    yPoint = imgUploadImageHeight - wmHeight - 10;
                    break;
                default://右下角
                    xPoint = imgUploadImageWidth - wmWidth - 10;
                    yPoint = 10;
                    break;
            }

            //将水印画到原图上，即 将 imgWaterMark 画到 imgUploadImage 上
            grWatermark.DrawImage(imgWaterMark, new Rectangle(xPoint, yPoint, wmWidth, wmHeight),
                0, 0, wmWidth, wmHeight, GraphicsUnit.Pixel);

            ImageCodecInfo ici = GetImageEncoder("JPEG");
            if (ici != null)
                imgUploadImage.Save(destImageUrl, ici, GetEncoderParameters(90));
            else
                imgUploadImage.Save(destImageUrl, ImageFormat.Jpeg);

            grWatermark.Dispose(); bmWaterMark.Dispose(); bmUploadImage.Dispose(); imgUploadImage.Dispose();

            imgUploadImage.Dispose();
        }


        /// <summary>
        ///  生成缩略图
        /// </summary>
        /// <param name="largeImagePhysicsPath">物理路径 E:\Cnkoo\Cnkoo\Cnkoo.Shop\UploadImage\200822</param>
        /// <param name="thumbnailImagePhysicsPath">缩略图物理路径 E:\Cnkoo\Cnkoo\Cnkoo.Shop\UploadImage\200822</param>
        /// <param name="fileName">文件名 20402714619.jpg</param>
        /// <param name="smallFileName">缩略图文件名 20402714619__s.jpg</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static void ImageThumbnail(string sourceImageUrl, string destImageUrl, int width, int height)
        {
            Image image = Image.FromFile(sourceImageUrl);

            //按比例计算出缩略图的宽度和高度
            //利用等比性质求缩略图的新宽度：因为 100 / 80 = 10 / x，所以 x = 80 * 10 / 100
            int _width = image.Width;
            int _height = image.Height;

            if (_width >= _height)
                height = (int)Math.Floor(System.Convert.ToDouble(_height) * (System.Convert.ToDouble(width) / System.Convert.ToDouble(_width)));
            else
                width = (int)Math.Floor(System.Convert.ToDouble(_width) * (System.Convert.ToDouble(height) / System.Convert.ToDouble(_height)));

            Bitmap bm = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bm);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, new Rectangle(0, 0, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

            ImageCodecInfo ici = GetImageEncoder("JPEG");

            if (ici != null)
                bm.Save(destImageUrl, ici, GetEncoderParameters(90));
            else
                bm.Save(destImageUrl, ImageFormat.Jpeg);

            bm.Dispose(); g.Dispose(); image.Dispose();
        }

        /// <summary>
        ///  生成缩略图
        /// </summary>
        /// <param name="sourcePhysicsPath">源物理路径 E:\Cnkoo\Cnkoo\Cnkoo.Shop\UploadImage\200822\</param>
        /// <param name="targetPhysicsPath">目的物理路径 E:\Cnkoo\Cnkoo\Cnkoo.Shop\UploadImage\200822\</param>
        /// <param name="sourceFileName">文件名 20402714619.jpg</param>
        /// <param name="targetFileName">缩略图文件名 20402714619__s.jpg</param>
        /// <param name="newWidth">宽</param>
        /// <param name="newHeight">高，指定高度则是等比；输入0为固定宽度</param>
        /// <returns></returns>
        public static void ThumbnailImage(string sourcePhysicsPath, string targetPhysicsPath, string sourceFileName, string targetFileName, int newWidth, int newHeight)
        {
            Image image = Image.FromFile(sourcePhysicsPath + sourceFileName);

            //按比例计算出缩略图的宽度和高度 开始
            //利用等比性质求缩略图的新宽度：因为 100 / 80 = 10 / x，所以 x = 80 * 10 / 100
            int width = image.Width;
            int height = image.Height;

            if (width >= height)
            {
                newHeight = (int)Math.Floor(Convert.ToDouble(height) * (Convert.ToDouble(newWidth) / Convert.ToDouble(width)));
            }
            else
            {
                if (newHeight == 0)//固定宽度
                {
                    newHeight = (int)Math.Floor(Convert.ToDouble(height) * (Convert.ToDouble(newWidth) / Convert.ToDouble(width)));
                }
                else
                {
                    newWidth = (int)Math.Floor(Convert.ToDouble(width) * (Convert.ToDouble(newHeight) / Convert.ToDouble(height)));
                }
            }
            //按比例计算出缩略图的宽度和高度 结束

            Bitmap b = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(b);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

            //以下代码为保存图片时，设置压缩质量
            //EncoderParameters encoderParams = new EncoderParameters();
            //long[] quality = new long[1];
            //quality[0] = 80;

            //EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //encoderParams.Param[0] = encoderParam;

            ImageCodecInfo ici = GetImageEncoder("JPEG");
            if (ici != null)
                b.Save(targetPhysicsPath + targetFileName, ici, GetEncoderParameters(90));
            else
                b.Save(targetPhysicsPath + targetFileName, ImageFormat.Jpeg);
            
            image.Dispose(); b.Dispose(); g.Dispose();

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            //ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            //ImageCodecInfo jpegICI = null;
            //for (int x = 0; x < arrayICI.Length; x++)
            //{
            //    if (arrayICI[x].FormatDescription.Equals("JPEG"))
            //    {
            //        jpegICI = arrayICI[x];//设置JPEG编码
            //        break;
            //    }
            //}

            //if (jpegICI != null)
            //{
            //    try
            //    { b.Save(targetPhysicsPath + targetFileName, jpegICI, encoderParams); }
            //    catch
            //    { throw; }
            //    finally
            //    { image.Dispose(); b.Dispose(); g.Dispose(); }

            //}
            //else
            //{
            //    try
            //    { b.Save(targetPhysicsPath + targetFileName, ImageFormat.Jpeg); }
            //    catch
            //    { throw; }
            //    finally
            //    { image.Dispose(); b.Dispose(); g.Dispose(); }
            //}
        }

        /// <summary>
        /// EncoderParameters
        /// </summary>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.EncoderParameters GetEncoderParameters(int quality)
        {
            EncoderParameters ep = new EncoderParameters();
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new long[1] { quality });// 90%
            return ep;
        }

        /// <summary>
        /// ImageCodecInfo
        /// </summary>
        /// <param name="formatDescription"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageCodecInfo GetImageEncoder(string formatDescription)
        {
            ImageCodecInfo[] ici = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < ici.Length; i++)
            {
                if (ici[i].FormatDescription.Equals(formatDescription)) return ici[i];
            }
            return null;
        }

        /// <summary>
        /// 在原图底部增加一点高度，画上logo等
        /// DotNet.Drawing.Imaging.AddWatermarkToImg(file.InputStream, path + fileName, 
        ///                        Server.MapPath("/") + @"/imgbds/2013/04/watermarkleft.png", Server.MapPath("/") + @"/imgbds/2013/04/watermarkright.png",
        ///                        30, 2);
        /// </summary>
        /// <param name="stream">上传文件的流</param>
        /// <param name="targetPhysicsPath">保存到那个路径</param>
        /// <param name="waterMarkUrl1"></param>
        /// <param name="waterMarkUrl2"></param>
        /// <param name="height">增加一点的高度</param>
        /// <param name="padding">上下左右空出多少</param>
        public static void AddWatermarkToImg(System.IO.Stream stream, string targetPhysicsPath, string waterMarkUrl1, string waterMarkUrl2, int height, int padding)
        {
            //载入底图 
            Image fromImage = Image.FromStream(stream);

            //水印
            Image imgWaterMark1 = new Bitmap(waterMarkUrl1);
            Image imgWaterMark2 = new Bitmap(waterMarkUrl2);

            Rectangle destR2 = new Rectangle(new Point(0, 0), new Size(fromImage.Width, fromImage.Height));//新图大小（选中的框的大小）
            Rectangle origR2 = new Rectangle(new Point(0, 0), new Size(fromImage.Width, fromImage.Height));//原图位置（选中的框内的那块）


            //创建新图位图   
            Bitmap bitmap = new Bitmap(fromImage.Width, fromImage.Height + height);//比原图高一点的

            //创建作图区域   
            Graphics graphic = Graphics.FromImage(bitmap);

            graphic.DrawImage(fromImage, destR2, origR2, GraphicsUnit.Pixel);
            //graphic.DrawImage(fromImage, new Point(0, 0));

            //分别将两个水印画到原图上
            graphic.DrawImage(imgWaterMark1, new Rectangle(0 + 2, fromImage.Height + padding, imgWaterMark1.Width, imgWaterMark1.Height),
                0, 0, imgWaterMark1.Width, imgWaterMark1.Height, GraphicsUnit.Pixel);
            graphic.DrawImage(imgWaterMark2, new Rectangle(fromImage.Width - imgWaterMark2.Width - padding, fromImage.Height + padding, imgWaterMark2.Width, imgWaterMark2.Height),
                0, 0, imgWaterMark2.Width, imgWaterMark2.Height, GraphicsUnit.Pixel);

            //从作图区生成新图   
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());

            //保存图象   
            saveImage.Save(targetPhysicsPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            //释放资源   
            saveImage.Dispose();
            bitmap.Dispose();
            graphic.Dispose();
            imgWaterMark1.Dispose();
            imgWaterMark2.Dispose();
            fromImage.Dispose();
        }

        ///   <summary>   
        ///   从图片中截取一个最大面积的正方形，生成新图，注意新图是正方形，不是正方形比例会乱   
        ///   </summary>   
        ///   <param   name="sourcePhysicsPath">源物理路径 E:\Cnkoo\Cnkoo\Cnkoo.Shop\UploadImage\200822\</param>   
        ///   <param   name="targetPhysicsPath">目的物理路径 E:\Cnkoo\Cnkoo\Cnkoo.Shop\UploadImage\200822\</param>   
        ///   <param   name="width">新图的宽</param>   
        ///   <param   name="height">新图的高</param>   
        public static void CaptureImage(string sourcePhysicsPath, string targetPhysicsPath, int width, int height)
        {
            //载入底图   
            Image fromImage = Image.FromFile(sourcePhysicsPath);
            int x = 0;   //截取X坐标   
            int y = 0;   //截取Y坐标 

            Rectangle destR2 = new Rectangle();
            Rectangle origR2 = new Rectangle();
            int newW = 0;
            newW = (fromImage.Width >= fromImage.Height ? fromImage.Height : fromImage.Width);
            if (fromImage.Width >= fromImage.Height)//横图
            {
                x = (fromImage.Width - fromImage.Height) / 2;

                y = 0;
                origR2 = new Rectangle(new Point(x, y), new Size(newW, newW));//原图位置（选中的框内的那块）
            }
            else if (fromImage.Width < fromImage.Height)//竖图
            {
                x = 0;
                y = (fromImage.Height - fromImage.Width) / 2;

                origR2 = new Rectangle(new Point(x, y), new Size(newW, newW));//原图位置（选中的框内的那块）
            }

            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域   
            Graphics graphic = Graphics.FromImage(bitmap);

            //截取原图相应区域写入作图区   
            //graphic.DrawImage(fromImage, 0, 0, new Rectangle(x, y, 300, 300), GraphicsUnit.Pixel);

            destR2 = new Rectangle(new Point(0, 0), new Size(width, height));//新图大小（选中的框的大小）

            graphic.DrawImage(fromImage, destR2, origR2, GraphicsUnit.Pixel);

            //从作图区生成新图   
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图象   
            saveImage.Save(targetPhysicsPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            //释放资源   
            saveImage.Dispose();
            bitmap.Dispose();
            graphic.Dispose();
        }
    }
}
