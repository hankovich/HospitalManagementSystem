namespace Hms.API.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR;

    public class NotificationHub : Hub
    {
        public void Hello()
        {
            Clients.All.Send(new ConnectionMessage()).hello();
            Clients.All.Hello();
        }

        public override Task OnConnected()
        {
            Clients.Caller.hello();
            return base.OnConnected();
        }
    }
}