using Hms.API;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Hms.API
{
    using System;
    using System.Web.Http;

    using Hms.Resolver;

    using Microsoft.AspNet.SignalR;

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

            var kernel = CreateKernel();

            appBuilder.UseNinjectMiddleware(() => kernel).UseNinjectWebApi(httpConfiguration);

            appBuilder.MapSignalR(new HubConfiguration
            {
                Resolver = new NinjectSignalRDependencyResolver(kernel)
            });
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
            kernel.Load(AppDomain.CurrentDomain.GetAssemblies());
            return kernel;
        }
    }
}