using Hms.API;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Hms.API
{
    using System;
    using System.Web.Http;

    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;

    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);

            appBuilder.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(httpConfiguration);

            appBuilder.MapSignalR();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
            kernel.Load(AppDomain.CurrentDomain.GetAssemblies());
            return kernel;
        }
    }
}