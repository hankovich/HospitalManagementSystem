namespace Hms.Resolver
{
    using Hms.Hubs;
    using Hms.Hubs.Interface;

    using Ninject.Modules;

    public class HubModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INotificationService>().To<NotificationHub>();
        }
    }
}
