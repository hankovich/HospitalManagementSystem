namespace Hms.Resolver
{
    using Hms.Hubs;
    using Hms.Hubs.Interface;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.AspNet.SignalR.Infrastructure;

    using Ninject.Modules;

    public class HubModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INotificationHub>().To<NotificationHub>().InSingletonScope();

            this.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context => new NinjectSignalRDependencyResolver(context.Kernel).Resolve<IConnectionManager>().GetHubContext<NotificationHub>().Clients).WhenInjectedInto<INotificationHub>();
        }
    }
}
