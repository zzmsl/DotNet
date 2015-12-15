using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Tools
{
    public class Page
    {
        /// <summary>
        /// 分页页码，一般用，如带?的
        /// 调用：PageNumber(total, pageSize, 1, System.IO.Path.GetFileName(Request.PhysicalPath) + "?page=");
        /// </summary>
        /// <param name="total"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="frontUrl"></param>
        /// <returns></returns>
        public static string PageNumber(int total, int pageSize, int page, string frontUrl)
        {            
            return PageNumber(total, pageSize, page, frontUrl, "", "", "", "", "");

            /****************正确的******************/

            #region 正确的           
            
            ////total 算出总页数
            ////page 当前页

            ////整除 37 / 10 = 3，
            //int j = total / pageSize;
            ////求余 37 / 10 = 7
            //int k = total % pageSize;

            ////37 应该有 4 页，所以要 +1
            //if (k > 0) total = j + 1; else total = j;

            //if (page < 1) page = 1;
            //if (page > total) page = total;

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //string url = "<a href=\"" + frontUrl;
            //string urlPrev = "<a href=\"" + frontUrl;
            //string urlNext = "<a href=\"" + frontUrl;
            //string urlCurrent = "<a href=\"" + frontUrl + page + "\"> " + page.ToString() + "</a> ";

            ////开始输出页数

            ////小于10页时
            //if (total <= 10)
            //{
            //    //从 2 到 num 时，添加 上一页 链接
            //    if (page > 1 & page <= total)
            //        sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

            //    //循环输出 num 个链接
            //    for (int i = 1; i <= total; i++)
            //    {
            //        //不为当前页时                    
            //        if (i != page)
            //            sb.Append(url + i + "\">" + i + "</a> ");
            //        else
            //            sb.Append(urlCurrent);
            //    }

            //    //从 1 到 num-1 页时，添加 下一页 链接
            //    if (page >= 1 & page < total)
            //        sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
            //}

            ////大于10页时
            //else
            //{
            //    //前面 5 页，+ 尾页
            //    if (page <= 5)
            //    {
            //        //第 1 页
            //        if (page == 1)
            //        {
            //            for (int i = 1; i <= 10; i++)
            //            {
            //                if (i != page)
            //                    sb.Append(url + i + "\">" + i + "</a> ");
            //                else
            //                    sb.Append(urlCurrent);
            //            }
            //            sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
            //        }
            //        else
            //        {
            //            sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");
            //            for (int i = 1; i <= 10; i++)
            //            {
            //                if (i != page)
            //                    sb.Append(url + i + "\">" + i + "</a> ");
            //                else
            //                    sb.Append(urlCurrent);
            //            }
            //            //最后页
            //            sb.Append("<span>...</span>" + url + total + "\">" + total + "</a> ");

            //            sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
            //        }
            //    }

            //    //中间x页， + 1页 尾页
            //    if (page > 5 && page <= total - 5)
            //    {
            //        sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

            //        //第一页
            //        sb.Append(url + 1 + "\">" + 1 + "</a> " + "<span>...</span>");

            //        for (int i = -5; i <= 5; i++)
            //        {
            //            //因为从 i 的中间分两边循环输出，所以中间那个不要加链接
            //            if (i != 0)
            //                sb.Append(url + (page + i) + "\">" + (page + i) + "</a> ");
            //            else
            //                sb.Append(urlCurrent);
            //        }
            //        //最后页
            //        sb.Append("<span>...</span>" + url + total + "\">" + total + "</a> ");

            //        sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
            //    }

            //    //后面5页， + 1页
            //    if (page > total - 5)
            //    {
            //        //最后页
            //        if (page == total)
            //        {
            //            sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

            //            //第一页
            //            sb.Append(url + 1 + "\">" + 1 + "</a> " + "<span>...</span>");

            //            for (int i = 10; i >= 0; i--)
            //            {
            //                if (i != 0)
            //                    sb.Append(url + (page - i) + "\">" + (page - i) + "</a> ");
            //                else
            //                    sb.Append(urlCurrent);
            //            }
            //        }
            //        //最后 5 页
            //        else
            //        {
            //            sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

            //            //第一页
            //            sb.Append(url + 1 + "\">" + 1 + "</a> " + "<span>...</span>");

            //            for (int i = 9; i >= 0; i--)
            //            {
            //                //重点语句，由于是循环 j 从 9-0显示 10个数，所以
            //                //num - i（总页数　-　当前页数）　＝ 倒数第 j 个时，刚好是 当前页数 所以不加链接
            //                //例如如： 10 11 12 13 14 15 16 17 18 19
            //                //设当前页：17
            //                //当 19 - 17 == 2时，此页就不加链接，即当前页。
            //                //所以 就是第 9 - 2 = 第 7 个不加链接
            //                if ((total - page) != i)
            //                    sb.Append(url + (total - i) + "\">" + (total - i) + "</a> ");
            //                else
            //                    sb.Append(urlCurrent);
            //            }

            //            sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
            //        }
            //    }
            //}
            //return sb.ToString();
            #endregion
        }

        public static string PageNumber(int total, int pageSize, int page, string frontUrl
              ,string pageCssClass
            ,string pagePrevCssClass
            ,string pageNextCssClass
            ,string pageCurrentCssClass
            , string ellipsisHtml )
        {
            //total 算出总页数
            //page 当前页

            //整除 37 / 10 = 3，
            int j = total / pageSize;
            //求余 37 / 10 = 7
            int k = total % pageSize;

            //37 应该有 4 页，所以要 +1
            if (k > 0) total = j + 1; else total = j;

            if (page < 1) page = 1;
            if (page > total) page = total;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string url = ""; string urlPrev = ""; string urlNext = ""; string urlCurrent = "";

            if (string.IsNullOrEmpty(frontUrl))
            { url = "<a href=\"" + frontUrl; }
            else
            { url = "<a class=\"" + pageCssClass + "\" href=\"" + frontUrl; }

            if (string.IsNullOrEmpty(urlPrev))
            { urlPrev = "<a href=\"" + frontUrl; }
            else
            { urlPrev = "<a class=\"" + pagePrevCssClass + "\" href=\"" + frontUrl; }

            if (string.IsNullOrEmpty(urlPrev))
            { urlPrev = "<a href=\"" + frontUrl; }
            else
            { urlPrev = "<a class=\"" + pagePrevCssClass + "\" href=\"" + frontUrl; }

            if (string.IsNullOrEmpty(urlNext))
            { urlNext = "<a href=\"" + frontUrl; }
            else
            { urlNext = "<a class=\"" + pageNextCssClass + "\" href=\"" + frontUrl; }

            if (string.IsNullOrEmpty(urlCurrent))
            { urlCurrent = "<a href=\"" + frontUrl + page + "\"> " + page.ToString() + "</a> "; }
            else
            { urlCurrent = "<a class=\"" + pageCurrentCssClass + "\" href=\"" + frontUrl + page + "\"> " + page.ToString() + "</a>"; }           

            //开始输出页数

            //小于10页时
            if (total <= 10)
            {
                //从 2 到 num 时，添加 上一页 链接
                if (page > 1 & page <= total)
                    sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

                //循环输出 num 个链接
                for (int i = 1; i <= total; i++)
                {
                    //不为当前页时                    
                    if (i != page)
                        sb.Append(url + i + "\">" + i + "</a> ");
                    else
                        sb.Append(urlCurrent);
                }

                //从 1 到 num-1 页时，添加 下一页 链接
                if (page >= 1 & page < total)
                    sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
            }

            //大于10页时
            else
            {
                //前面 5 页，+ 尾页
                if (page <= 5)
                {
                    //第 1 页
                    if (page == 1)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            if (i != page)
                                sb.Append(url + i + "\">" + i + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }
                        sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
                    }
                    else
                    {
                        sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");
                        for (int i = 1; i <= 10; i++)
                        {
                            if (i != page)
                                sb.Append(url + i + "\">" + i + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }
                        //最后页
                        sb.Append(ellipsisHtml + url + total + "\">" + total + "</a> ");

                        sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
                    }
                }

                //中间x页， + 1页 尾页
                if (page > 5 && page <= total - 5)
                {
                    sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

                    //第一页
                    sb.Append(url + 1 + "\">" + 1 + "</a> " + ellipsisHtml);

                    for (int i = -5; i <= 5; i++)
                    {
                        //因为从 i 的中间分两边循环输出，所以中间那个不要加链接
                        if (i != 0)
                            sb.Append(url + (page + i) + "\">" + (page + i) + "</a> ");
                        else
                            sb.Append(urlCurrent);
                    }
                    //最后页
                    sb.Append(ellipsisHtml + url + total + "\">" + total + "</a> ");

                    sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
                }

                //后面5页， + 1页
                if (page > total - 5)
                {
                    //最后页
                    if (page == total)
                    {
                        sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

                        //第一页
                        sb.Append(url + 1 + "\">" + 1 + "</a> " + ellipsisHtml);

                        for (int i = 10; i >= 0; i--)
                        {
                            if (i != 0)
                                sb.Append(url + (page - i) + "\">" + (page - i) + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }
                    }
                    //最后 5 页
                    else
                    {
                        sb.Append(urlPrev + (page - 1) + "\">上一页</a> ");

                        //第一页
                        sb.Append(url + 1 + "\">" + 1 + "</a> " + ellipsisHtml);

                        for (int i = 9; i >= 0; i--)
                        {
                            //重点语句，由于是循环 j 从 9-0显示 10个数，所以
                            //num - i（总页数　-　当前页数）　＝ 倒数第 j 个时，刚好是 当前页数 所以不加链接
                            //例如如： 10 11 12 13 14 15 16 17 18 19
                            //设当前页：17
                            //当 19 - 17 == 2时，此页就不加链接，即当前页。
                            //所以 就是第 9 - 2 = 第 7 个不加链接
                            if ((total - page) != i)
                                sb.Append(url + (total - i) + "\">" + (total - i) + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }

                        sb.Append(urlNext + (page + 1) + "\">下一页</a> ");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 分页页码，Url重写用
        /// 调用：DotNet.Text.PageNumberUrlRewriter(total, pageSize, page, "/" + cat + "/page-"
        /// ,"page","page-prev","page-next","page-current","<span>...</span>");
        /// </summary>
        /// <param name="total">总条数</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="page">第几页</param>
        /// <param name="frontUrl">其他相关的url参数</param>
        /// <param name="pageCssClass">一般页码链接的css class</param>
        /// <param name="pagePrevCssClass">上一页链接的css class</param>
        /// <param name="pageNextCssClass">下一页链接的css class</param>
        /// <param name="pageCurrentCssClass">当前页链接的css class</param>
        /// <param name="ellipsisHtml">省略号的html代码，如<span>...</span></param>
        /// <returns></returns>
        public static string PageNumberUrlRewriter(int total, int pageSize, int page, string frontUrl 
            ,string pageCssClass
            ,string pagePrevCssClass
            ,string pageNextCssClass
            ,string pageCurrentCssClass
            , string ellipsisHtml )
        {
            //total 算出总页数
            //page 当前页

            //整除 37 / 10 = 3，
            int j = total / pageSize;
            //求余 37 / 10 = 7
            int k = total % pageSize;

            //37 应该有 4 页，所以要 +1
            if (k > 0) total = j + 1; else total = j;

            if (page < 1) page = 1;
            if (page > total) page = total;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //string url = "<a class=\"page\" href=\"" + frontUrl;
            //string urlPrev = "<a class=\"page-prev\" href=\"" + frontUrl;
            //string urlNext = "<a class=\"page-next\" href=\"" + frontUrl;
            //string urlCurrent = "<a class=\"current\" href=\"" + frontUrl + page + ".aspx\"> " + page.ToString() + "</a>";
            string url = "<a class=\"" + pageCssClass + "\" href=\"" + frontUrl;
            string urlPrev = "<a class=\"" + pagePrevCssClass + "\" href=\"" + frontUrl;
            string urlNext = "<a class=\"" + pageNextCssClass + "\" href=\"" + frontUrl;
            string urlCurrent = "<a class=\"" + pageCurrentCssClass + "\" href=\"" + frontUrl + page + ".aspx\"> " + page.ToString() + "</a>";

            //开始输出页数

            //小于10页时
            if (total <= 10)
            {
                //从 2 到 num 时，添加 上一页 链接
                if (page > 1 & page <= total)
                    sb.Append(urlPrev + (page - 1) + ".aspx\">上页</a> ");

                //循环输出 num 个链接
                for (int i = 1; i <= total; i++)
                {
                    //不为当前页时                    
                    if (i != page)
                        sb.Append(url + i + ".aspx\">" + i + "</a> ");
                    else
                        sb.Append(urlCurrent);
                }

                //从 1 到 num-1 页时，添加 下一页 链接
                if (page >= 1 & page < total)
                    sb.Append(urlNext + (page + 1) + ".aspx\">下页</a> ");
            }

            //大于10页时
            else
            {
                //前面 5 页，+ 尾页
                if (page <= 5)
                {
                    //第 1 页
                    if (page == 1)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            if (i != page)
                                sb.Append(url + i + ".aspx\">" + i + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }
                        sb.Append(urlNext + (page + 1) + ".aspx\">下页</a> ");
                    }
                    else
                    {
                        sb.Append(urlPrev + (page - 1) + ".aspx\">上页</a> ");
                        for (int i = 1; i <= 10; i++)
                        {
                            if (i != page)
                                sb.Append(url + i + ".aspx\">" + i + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }
                        //最后页
                        sb.Append("<span>...</span>" + url + total + ".aspx\">" + total + "</a> ");

                        sb.Append(urlNext + (page + 1) + ".aspx\">下页</a> ");
                    }
                }

                //中间x页， + 1页 尾页
                if (page > 5 && page <= total - 5)
                {
                    sb.Append(urlPrev + (page - 1) + ".aspx\">上页</a> ");

                    //第一页
                    //sb.Append(url + 1 + ".aspx\">" + 1 + "</a> " + "<span>...</span>");
                    sb.Append(url + 1 + ".aspx\">" + 1 + "</a> " +  ellipsisHtml);

                    for (int i = -5; i <= 5; i++)
                    {
                        //因为从 i 的中间分两边循环输出，所以中间那个不要加链接
                        if (i != 0)
                            sb.Append(url + (page + i) + ".aspx\">" + (page + i) + "</a> ");
                        else
                            sb.Append(urlCurrent);
                    }
                    //最后页
                    //sb.Append("<span>...</span>" + url + total + ".aspx\">" + total + "</a> ");
                    sb.Append(ellipsisHtml + url + total + ".aspx\">" + total + "</a> ");

                    sb.Append(urlNext + (page + 1) + ".aspx\">下页</a> ");
                }

                //后面5页， + 1页
                if (page > total - 5)
                {
                    //最后页
                    if (page == total)
                    {
                        sb.Append(urlPrev + (page - 1) + ".aspx\">上页</a> ");

                        //第一页
                        //sb.Append(url + 1 + ".aspx\">" + 1 + "</a> " + "<span>...</span>");
                        sb.Append(url + 1 + ".aspx\">" + 1 + "</a> " + ellipsisHtml);

                        for (int i = 10; i >= 0; i--)
                        {
                            if (i != 0)
                                sb.Append(url + (page - i) + ".aspx\">" + (page - i) + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }
                    }
                    //最后 5 页
                    else
                    {
                        sb.Append(urlPrev + (page - 1) + ".aspx\">上页</a> ");

                        //第一页
                        //sb.Append(url + 1 + ".aspx\">" + 1 + "</a> " + "<span>...</span>");
                        sb.Append(url + 1 + ".aspx\">" + 1 + "</a> " + ellipsisHtml);

                        for (int i = 9; i >= 0; i--)
                        {
                            //重点语句，由于是循环 j 从 9-0显示 10个数，所以
                            //num - i（总页数　-　当前页数）　＝ 倒数第 j 个时，刚好是 当前页数 所以不加链接
                            //例如如： 10 11 12 13 14 15 16 17 18 19
                            //设当前页：17
                            //当 19 - 17 == 2时，此页就不加链接，即当前页。
                            //所以 就是第 9 - 2 = 第 7 个不加链接
                            if ((total - page) != i)
                                sb.Append(url + (total - i) + ".aspx\">" + (total - i) + "</a> ");
                            else
                                sb.Append(urlCurrent);
                        }

                        sb.Append(urlNext + (page + 1) + ".aspx\">下页</a> ");
                    }
                }
            }
            return sb.ToString();
        }
        
    }
}
