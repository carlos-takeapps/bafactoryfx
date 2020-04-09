using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using BAFactory.Fx.Samples.WebApiSecurity.WebApi;

[assembly: OwinStartup(typeof(Startup))]

namespace BAFactory.Fx.Samples.WebApiSecurity.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
