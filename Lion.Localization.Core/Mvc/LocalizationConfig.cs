using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Lion.Localization.Mvc;
using Lion.Localization.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;



[assembly: PreApplicationStartMethod(typeof(LocalizationConfig), "Start")]
namespace Lion.Localization.Mvc
{
    public static class LocalizationConfig
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(LocalizationModule));
            Register();
        }

        public static void Register()
        {
            Register(GlobalConfiguration.Configuration);
        }

        public static void Register(HttpConfiguration config)
        {
            // configure localization of models
            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new ValidationLocalizer());
            ModelMetadataProviders.Current = new MetadataLocalizer();
        }
    }
}

