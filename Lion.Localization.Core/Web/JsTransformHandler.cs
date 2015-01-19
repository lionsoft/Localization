using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace Lion.Localization.Web
{
    public class JsTransformHandler : IHttpHandler
    {
        public class JsLocalizationTransform : IItemTransform
        {
            public static IItemTransform Instance = new JsLocalizationTransform();
            public string Process(string includedVirtualPath, string input)
            {
                var path = includedVirtualPath.Replace('\\', '/');

                /*
                            var regex = new Regex("__(?:R|R2)\\([\"']{1}.*?(?:[\"']{1}|[}]{1})\\)", RegexOptions.Compiled);
                            var script = regex.Replace(input, delegate(Match match)
                            {
                                var resource = match.Value.Replace("__R", "$.R");
                                resource = resource.Substring(0, resource.IndexOf('(') + 1) + "'" + path + "', " + resource.Substring(resource.IndexOf('(') + 1);

                                if (Path.GetExtension(path) == ".htm")
                                {
                                    if (resource.Contains("$.R2"))
                                        return "{{html " + resource + "}}";
                                    if (resource.Contains("$.R"))
                                        return "{{= " + resource + "}}";
                                }
                                return resource;
                            });
                */

                var regex = new Regex("([\"']{1}.*[\"']{1}).(?:L|L2)\\(\\)", RegexOptions.Compiled);
                var script = regex.Replace(input, delegate(Match match)
                {
                    var resource = string.Format("$.L('{0}', {1})", path, match.Groups[1]);

                    if (Path.GetExtension(path) == ".htm")
                    {
                        if (resource.Contains("$.L2"))
                            return "{{html " + resource + "}}";
                        if (resource.Contains("$.L"))
                            return "{{= " + resource + "}}";
                    }
                    return resource;
                });

                return script;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var virtualFileName = context.Request.AppRelativeCurrentExecutionFilePath;
            var fileName = HttpContext.Current.Server.MapPath(virtualFileName);
            if (File.Exists(fileName))
            {
                try
                {
                    var res = File.ReadAllText(fileName);
                    if (fileName.EndsWith(".js") || fileName.EndsWith(".ts"))
                        res = JsLocalizationTransform.Instance.Process(virtualFileName, res);
                    context.Response.Write(res);
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(e.Message);
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "text/plain";
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}