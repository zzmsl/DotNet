using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Web
{
    public class Submit
    {
        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        private static string BuildRequestParaToString(Dictionary<string, string> para, Encoding encoding)
        {
            ////待签名请求参数数组
            //Dictionary<string, string> sPara = new Dictionary<string, string>();
            //sPara = BuildRequestPara(sParaTemp);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = DotNet.Tools.Utility.CreateLinkStringUrlEncode(para, encoding);

            return strRequestData;
        }

        /// <summary>
        /// 构造提交表单HTML数据
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="action">网关地址</param>
        /// <param name="method">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildFormHtml(Dictionary<string, string> para, string formName, string action, string method)
        {
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='" + formName + "' name='" + formName + "' action='" + action + "' method='" + method.ToLower() + "'>");

            foreach (KeyValuePair<string, string> temp in para)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='点击提交' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['" + formName + "'].submit();</script>");

            return sbHtml.ToString();
        }
    }
}
