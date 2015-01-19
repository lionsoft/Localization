using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Lion.Localization.Web
{
	public class LocalizationModule: IHttpModule
	{	
		public void Init(HttpApplication context)
		{
			context.BeginRequest += BeginRequest;
            context.PostMapRequestHandler += PostMapRequestHandler;
		}

	    public static string FolderPatternTemplate = @"^\/{0}\/.*\.(js|ts)$";
        public static string BundlePatternTemplate = @"^\/{0}.*$";

        /// <summary>
        /// Регулярное выражение задающее
        /// </summary>
        public static readonly List<Regex> LocalizableScriptTemplateRegexes = new List<Regex>();

	    public static void AddLocalizableScriptTemplate(string pattern)
	    {
            LocalizableScriptTemplateRegexes.Add(new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));
	    }
        public static void AddLocalizableScriptFolder(string folder)
        {
            AddLocalizableScriptTemplate(string.Format(FolderPatternTemplate, Regex.Escape(folder)));
        }
        public static void AddLocalizableScriptBundle(string bundleName)
        {
            AddLocalizableScriptTemplate(string.Format(BundlePatternTemplate, Regex.Escape(bundleName)));
        }

	    static LocalizationModule()
	    {
            AddLocalizableScriptFolder("AppScripts");
            AddLocalizableScriptBundle("bundles/app");
        }

	    void PostMapRequestHandler(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Request.Url.AbsolutePath.StartsWith("/_localization/"))
            {
               context.Handler = new LocalizationHandler();
            }
            else if (LocalizableScriptTemplateRegexes.Any(x => x.IsMatch(context.Request.Url.AbsolutePath))) 
            {
                var filter = new ResponseFilterStream(context.Response.Filter);
                filter.TransformWriteString += s => JsTransformHandler.JsLocalizationTransform.Instance.Process(context.Request.AppRelativeCurrentExecutionFilePath, s);
                context.Response.Filter = filter;
            }
        }

		private void BeginRequest(Object sender, EventArgs e)
		{
			if (LocalizationManager.Repository != null)
			{
				var context = ((HttpApplication)sender).Context;
				var lang = string.Empty;

				// try to get language from request query, cookie or browser language

				if (context.Request.QueryString["lang"] != null)				
					lang = context.Request.QueryString["lang"];		
				
				else if (context.Request.Cookies[LocalizationManager.CookieName] != null)
					lang = context.Request.Cookies[LocalizationManager.CookieName].Value;

				else if (context.Request.UserLanguages != null)
				{
					lang = context.Request.UserLanguages[0];
					if (lang.Length < 3)
						lang = string.Format("{0}-{1}", lang, lang.ToUpper());
				}

				try
				{
					LocalizationManager.Instance.SetCulture(new CultureInfo(lang));
				}
				catch (CultureNotFoundException) 
				{ }
				catch (ArgumentNullException)
				{ }
			}
		}

		public void Dispose() { }		
	}
}
