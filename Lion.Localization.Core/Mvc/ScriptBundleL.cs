using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web.Optimization;
using Lion.Localization.Web;

namespace Lion.Localization.Mvc
{
    public class ScriptBundleL : ScriptBundle
    {
        public ScriptBundleL(string virtualPath) : this(virtualPath, null)
        {
        }

        public ScriptBundleL(string virtualPath, string cdnPath) : base(virtualPath, cdnPath)
        {
        }


        public override Bundle Include(string virtualPath, params IItemTransform[] transforms)
        {
            return base.Include(virtualPath, (new IItemTransform[] { new JsTransformHandler.JsLocalizationTransform() }.Union(transforms)).ToArray());
        }

        public override Bundle IncludeDirectory(string directoryVirtualPath, string searchPattern, bool searchSubdirectories)
        {
            var res = base.IncludeDirectory(directoryVirtualPath, searchPattern, searchSubdirectories);

//            res.Items[0].Transforms.Insert(0, new JsLocalizationTransform());
            var pi = res.GetType().GetProperty("Items", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (pi != null)
            {
                var items = pi.GetValue(res) as IList;
                if (items != null)
                {
                    foreach (var bundleItem in items.OfType<object>())
                    {
                        var pi1 = bundleItem.GetType().GetProperty("Transforms", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        if (pi1 != null)
                        {
                            var transforms = pi1.GetValue(bundleItem) as IList;
                            if (transforms != null)
                                transforms.Insert(0, new JsTransformHandler.JsLocalizationTransform());
                        }
                    }
                }
            }
            return res;
        }
    }
}