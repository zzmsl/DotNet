using System;
//using System.Collections.Generic;
//using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using DotNet.Configuration.URLRewriterConfig;

namespace DotNet.Web.URLRewriter
{
    /// <summary>
    /// The base class for module rewriting.  This class is abstract, and therefore must be derived from.
    /// </summary>
    /// <remarks>Provides the essential base functionality for a rewriter using the HttpModule approach.</remarks>
    public abstract class BaseModuleRewriter : IHttpModule
    {
        /// <summary>
        /// Executes when the module is initialized.
        /// </summary>
        /// <param name="app">A reference to the HttpApplication object processing this request.</param>
        /// <remarks>Wires up the HttpApplication's AuthorizeRequest event to the
        /// <see cref="BaseModuleRewriter_AuthorizeRequest"/> event handler.</remarks>
        public virtual void Init(HttpApplication app)
        {
            // WARNING!  This does not work with Windows authentication!
            // If you are using Windows authentication, change to app.BeginRequest
            app.AuthorizeRequest += new EventHandler(this.BaseModuleRewriter_AuthorizeRequest);
        }

        public virtual void Dispose() { }

        /// <summary>
        /// Called when the module's AuthorizeRequest event fires.
        /// </summary>
        /// <remarks>This event handler calls the <see cref="Rewrite"/> method, passing in the
        /// <b>RawUrl</b> and HttpApplication passed in via the <b>sender</b> parameter.</remarks>
        protected virtual void BaseModuleRewriter_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            
            //就是这里
            Rewrite(app.Request.Path, app);
            //Rewrite(app.Request.Url.AbsoluteUri, app);
        }

        /// <summary>
        /// The <b>Rewrite</b> method must be overriden.  It is where the logic for rewriting an incoming
        /// URL is performed.
        /// </summary>
        /// <param name="requestedRawUrl">The requested RawUrl.  (Includes full path and querystring.)</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected abstract void Rewrite(string requestedPath, HttpApplication app);
    }

    /// <summary>
    /// Provides a rewriting HttpModule.
    /// </summary>
    public class ModuleRewriter : BaseModuleRewriter
    {
        /// <summary>
        /// This method is called during the module's BeginRequest event.
        /// </summary>
        /// <param name="requestedRawUrl">The RawUrl being requested (includes path and querystring).</param>
        /// <param name="app">The HttpApplication instance.</param>
        protected override void Rewrite(string requestedPath, System.Web.HttpApplication app)
        {
            // log information to the Trace object.
            app.Context.Trace.Write("ModuleRewriter", "Entering ModuleRewriter");

            // get the configuration rules
            RewriterRuleCollection rules = RewriterConfiguration.GetConfig().Rules;

            // iterate through each rule...
            for (int i = 0; i < rules.Count; i++)
            {
                // get the pattern to look for, and Resolve the Url (convert ~ into the appropriate directory)
                
                //就是这里
                //string lookFor = "^" + rules[i].LookFor + "$";
                string lookFor = "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, rules[i].LookFor) + "$";

                // Create a regex (note that IgnoreCase is set...)
                Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                // See if a match is found
                if (re.IsMatch(requestedPath))
                {
                    // match found - do any replacement needed
                    string sendToUrl = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(requestedPath, rules[i].SendTo));

                    // log rewriting information to the Trace object
                    app.Context.Trace.Write("ModuleRewriter", "Rewriting URL to " + sendToUrl);

                    // Rewrite the URL
                    RewriterUtils.RewriteUrl(app.Context, sendToUrl);
                    break;		// exit the for loop
                }
            }

            // Log information to the Trace object
            app.Context.Trace.Write("ModuleRewriter", "Exiting ModuleRewriter");
        }
    }

    /// <summary>
    /// Provides utility helper methods for the rewriting HttpModule and HttpHandler.
    /// </summary>
    /// <remarks>This class is marked as internal, meaning only classes in the same assembly will be
    /// able to access its methods.</remarks>
    internal class RewriterUtils
    {
        #region RewriteUrl
        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        internal static void RewriteUrl(System.Web.HttpContext context, string sendToUrl)
        {
            string x, y;
            RewriteUrl(context, sendToUrl, out x, out y);
        }

        /// <summary>
        /// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
        /// </summary>
        /// <param name="context">The HttpContext object to rewrite the URL to.</param>
        /// <param name="sendToUrl">The URL to rewrite to.</param>
        /// <param name="sendToUrlLessQString">Returns the value of sendToUrl stripped of the querystring.</param>
        /// <param name="filePath">Returns the physical file path to the requested page.</param>
        internal static void RewriteUrl(System.Web.HttpContext context, string sendToUrl, out string sendToUrlLessQString, out string filePath)
        {
            // see if we need to add any extra querystring information
            if (context.Request.QueryString.Count > 0)
            {
                if (sendToUrl.IndexOf('?') != -1)
                    sendToUrl += "&" + context.Request.QueryString.ToString();
                else
                    sendToUrl += "?" + context.Request.QueryString.ToString();
            }

            // first strip the querystring, if any
            string queryString = String.Empty;
            sendToUrlLessQString = sendToUrl;
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
            }

            // grab the file's physical path
            filePath = string.Empty;
            filePath = context.Server.MapPath(sendToUrlLessQString);

            // rewrite the path...
            context.RewritePath(sendToUrlLessQString, String.Empty, queryString);

            // NOTE!  The above RewritePath() overload is only supported in the .NET Framework 1.1
            // If you are using .NET Framework 1.0, use the below form instead:
            // context.RewritePath(sendToUrl);
        }
        #endregion

        /// <summary>
        /// Converts a URL into one that is usable on the requesting client.
        /// </summary>
        /// <remarks>Converts ~ to the requesting application path.  Mimics the behavior of the 
        /// <b>Control.ResolveUrl()</b> method, which is often used by control developers.</remarks>
        /// <param name="appPath">The application path.</param>
        /// <param name="url">The URL, which might contain ~.</param>
        /// <returns>A resolved URL.  If the input parameter <b>url</b> contains ~, it is replaced with the
        /// value of the <b>appPath</b> parameter.</returns>
        internal static string ResolveUrl(string appPath, string url)
        {
            if (url.Length == 0 || url[0] != '~')
                return url;		// there is no ~ in the first character position, just return the url
            else
            {
                if (url.Length == 1)
                    return appPath;  // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }
    }
}
