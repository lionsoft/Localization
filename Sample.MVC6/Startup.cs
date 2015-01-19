using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Owin;

[assembly: OwinStartup(typeof(Sample.MVC6.Startup))]

namespace Sample.MVC6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
