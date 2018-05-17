namespace Hms.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using Ninject;

    public class NinjectSignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectSignalRDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override object GetService(Type serviceType)
        {
            var service = base.GetService(serviceType);

            if (service != null)
            {
                return service;
            }

            return this.kernel.TryGet(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}