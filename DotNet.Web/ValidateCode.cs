using System;
using System.Drawing;
using System.Web;

namespace Marry.Component
{
    /// <summary>
    /// 生成验证码
    /// aspx 的 Page_Load 使用方法
    /// Components.ValidateCode.DrawImage();
    /// </summary>
    public class ValidateCode
    {
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        public static void DrawImage()
        {
            ValidateCode vc = new ValidateCode();
            HttpContext.Current.Session["CheckValidateCode"] = vc.RandomString(4);
            vc.CreateImages(HttpContext.Current.Session["CheckValidateCode"].ToString());
        }

        /// <summary>
        /// 生成验证图片
        /// </summary>
        /// <param name="validateCode">验证字符</param>
        private void CreateImages(string validateCode)
        {
            int width = (int)(validateCode.Length * 14);
            Bitmap image = new Bitmap(width, 23);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] color = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, 
            Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体 
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random ran = new Random();
            //随机输出噪点
            for (int i = 0; i < 25; i++)
            {
                // i 求余为 0，即是偶数
                if (i % 2 == 0)
                {
                    int x = ran.Next(image.Width);
                    int y = ran.Next(image.Height);
                    g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
                }
                else
                {
                    // i 为素数
                    int x = ran.Next(image.Width);
                    int y = ran.Next(image.Height);
                    g.DrawRectangle(new Pen(Color.PaleGreen, 0), x, y, 1, 1);
                }
            }

            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < validateCode.Length; i++)
            {
                //8 种颜色
                int colorIndex = ran.Next(8);
                //5 种字体
                int fontIndex = ran.Next(5);
                // 得到第 i 的那个字符
                string s = validateCode.Substring(i, 1);

                Font f = new Font(font[fontIndex], 12, System.Drawing.FontStyle.Bold);

                Brush b = new SolidBrush(color[colorIndex]);

                //所绘制文本的左上角的 x 坐标。 
                int x = i * 12 + 3;

                //所绘制文本的左上角的 y 坐标。
                int y = ran.Next(5);

                g.DrawString(s, f, b, x, y);
            }

            //画一个边框
            g.DrawRectangle(new Pen(Color.Orange, 0), 0, 0, image.Width - 1, image.Height - 1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.AddHeader("Pragma", "No-Cache");
            HttpContext.Current.Response.AddHeader("Cache-Control", "No-Cache");
            HttpContext.Current.Response.ContentType = "Image/Jpeg";
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 生成验证码的随机字符
        /// </summary>
        /// <param name="number">生成字符的个数</param>
        /// <returns>string</returns>
        private string RandomString(int number)
        {
            string[] randomStringArray = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };
            string randomString = "";
            int temp = -1; //记录上次随机数值，避免生产几个一样的随机数

            //保证生成随机数的不同
            Random ran = new Random();
            for (int i = 1; i < number + 1; i++)
            {
                if (temp != -1)
                {
                    ran = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = ran.Next(randomStringArray.Length);
                if (temp != -1 && temp == t)
                {
                    //递归，重来一次
                    return RandomString(number);
                }
                temp = t;
                randomString += randomStringArray[t];
            }
            return randomString;
        }
    }
}
