using System;
using System.Web;
using System.Configuration;
using System.Xml.Serialization;

using System.Xml;
using System.Collections;

namespace DotNet.Configuration.URLRewriterConfig
{
    /// <summary>
    /// Deserializes the markup in Web.config into an instance of the <see cref="RewriterConfiguration"/> class.
    /// </summary>
    public class RewriterConfigSerializerSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates an instance of the <see cref="RewriterConfiguration"/> class.
        /// </summary>
        /// <remarks>Uses XML Serialization to deserialize the XML in the Web.config file into an
        /// <see cref="RewriterConfiguration"/> instance.</remarks>
        /// <returns>An instance of the <see cref="RewriterConfiguration"/> class.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // Create an instance of XmlSerializer based on the RewriterConfiguration type...
            XmlSerializer ser = new XmlSerializer(typeof(RewriterConfiguration));

            // Return the Deserialized object from the Web.config XML
            return ser.Deserialize(new XmlNodeReader(section));
        }

    }


    /// <summary>
    /// Specifies the configuration settings in the Web.config for the RewriterRule.
    /// </summary>
    /// <remarks>This class defines the structure of the Rewriter configuration file in the Web.config file.
    /// Currently, it allows only for a set of rewrite rules; however, this approach allows for customization.
    /// For example, you could provide a ruleset that <i>doesn't</i> use regular expression matching; or a set of
    /// constant names and values, which could then be referenced in rewrite rules.
    /// <p />
    /// The structure in the Web.config file is as follows:
    /// <code>
    /// &lt;configuration&gt;
    /// 	&lt;configSections&gt;
    /// 		&lt;section name="RewriterConfig" 
    /// 		            type="URLRewriter.Config.RewriterConfigSerializerSectionHandler, URLRewriter" /&gt;
    ///		&lt;/configSections&gt;
    ///		
    ///		&lt;RewriterConfig&gt;
    ///			&lt;Rules&gt;
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///				...
    ///				&lt;RewriterRule&gt;
    ///					&lt;LookFor&gt;<i>pattern</i>&lt;/LookFor&gt;
    ///					&lt;SendTo&gt;<i>replace with</i>&lt;/SendTo&gt;
    ///				&lt;/RewriterRule&gt;
    ///			&lt;/Rules&gt;
    ///		&lt;/RewriterConfig&gt;
    ///		
    ///		&lt;system.web&gt;
    ///			...
    ///		&lt;/system.web&gt;
    ///	&lt;/configuration&gt;
    /// </code>
    /// </remarks>
    [Serializable()]
    [XmlRoot("RewriterConfig")]
    public class RewriterConfiguration
    {
        // private member variables
        private RewriterRuleCollection rules;			// an instance of the RewriterRuleCollection class...

        /// <summary>
        /// GetConfig() returns an instance of the <b>RewriterConfiguration</b> class with the values populated from
        /// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
        /// a <b>RewriterConfiguration</b> instance.
        /// </summary>
        /// <returns>A <see cref="RewriterConfiguration"/> instance.</returns>
        public static RewriterConfiguration GetConfig()
        {
            if (HttpContext.Current.Cache["RewriterConfig"] == null)
                //HttpContext.Current.Cache.Insert("RewriterConfig", ConfigurationSettings.GetConfig("RewriterConfig"));
                HttpContext.Current.Cache.Insert("RewriterConfig", ConfigurationManager.GetSection("RewriterConfig"));

            return (RewriterConfiguration)HttpContext.Current.Cache["RewriterConfig"];
        }

        #region Public Properties
        /// <summary>
        /// A <see cref="RewriterRuleCollection"/> instance that provides access to a set of <see cref="RewriterRule"/>s.
        /// </summary>
        public RewriterRuleCollection Rules
        {
            get
            {
                return rules;
            }
            set
            {
                rules = value;
            }
        }
        #endregion
    }

    /// <summary>
    /// Represents a rewriter rule.  A rewriter rule is composed of a pattern to search for and a string to replace
    /// the pattern with (if matched).
    /// </summary>
    [Serializable()]
    public class RewriterRule
    {
        // private member variables...
        private string lookFor, sendTo;

        #region Public Properties
        /// <summary>
        /// Gets or sets the pattern to look for.
        /// </summary>
        /// <remarks><b>LookFor</b> is a regular expression pattern.  Therefore, you might need to escape
        /// characters in the pattern that are reserved characters in regular expression syntax (., ?, ^, $, etc.).
        /// <p />
        /// The pattern is searched for using the <b>System.Text.RegularExpression.Regex</b> class's <b>IsMatch()</b>
        /// method.  The pattern is case insensitive.</remarks>
        public string LookFor
        {
            get
            {
                return lookFor;
            }
            set
            {
                lookFor = value;
            }
        }

        /// <summary>
        /// The string to replace the pattern with, if found.
        /// </summary>
        /// <remarks>The replacement string may use grouping symbols, like $1, $2, etc.  Specifically, the
        /// <b>System.Text.RegularExpression.Regex</b> class's <b>Replace()</b> method is used to replace
        /// the match in <see cref="LookFor"/> with the value in <b>SendTo</b>.</remarks>
        public string SendTo
        {
            get
            {
                return sendTo;
            }
            set
            {
                sendTo = value;
            }
        }
        #endregion
    }


    /// <summary>
    /// The RewriterRuleCollection models a set of RewriterRules in the Web.config file.
    /// </summary>
    /// <remarks>
    /// The RewriterRuleCollection is expressed in XML as:
    /// <code>
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// ...
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// </code>
    /// </remarks>
    [Serializable()]
    public class RewriterRuleCollection : CollectionBase
    {
        /// <summary>
        /// Adds a new RewriterRule to the collection.
        /// </summary>
        /// <param name="r">A RewriterRule instance.</param>
        public virtual void Add(RewriterRule r)
        {
            this.InnerList.Add(r);
        }

        /// <summary>
        /// Gets or sets a RewriterRule at a specified ordinal index.
        /// </summary>
        public RewriterRule this[int index]
        {
            get
            {
                return (RewriterRule)this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = value;
            }
        }
    }
}
