namespace Hms.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hms.Hubs.Interface;
    using Hms.Services.Interface;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    public class NotificationHub : Hub, INotificationHub
    {
        private static readonly ConcurrentDictionary<string, string> ConnectionToGadgetMappings =
            new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<int, ICollection<ObserverInfo>> TimetableObservers =
            new ConcurrentDictionary<int, ICollection<ObserverInfo>>();

        public NotificationHub(IHubConnectionContext<dynamic> clients, IUserSessionService userSessionService)
        {
            this.InjectedClients = clients;
            this.UserSessionService = userSessionService;
        }

        public IHubConnectionContext<dynamic> InjectedClients { get; }

        public IUserSessionService UserSessionService { get; }

        public override Task OnDisconnected(bool stopCalled)
        {
            string result = string.Empty;
            ConnectionToGadgetMappings.TryRemove(this.Context.ConnectionId, out result);

            return base.OnDisconnected(stopCalled);
        }

        public void ConnectDoctor(string identifier)
        {
            ConnectionToGadgetMappings.AddOrUpdate(this.Context.ConnectionId, s => identifier, (s, s1) => identifier);
        }

        public void ConnectToTimetable(int doctorId, DateTime date)
        {
            var observerInfo = new ObserverInfo(Context.ConnectionId, date);

            TimetableObservers.AddOrUpdate(
                doctorId,
                id => new List<ObserverInfo> { observerInfo },
                (id, pairs) =>
                {
                    pairs.Add(observerInfo);
                    return pairs;
                });
        }

        public void DisconnectFromTimetable(int doctorId, DateTime date)
        {
            var observerInfo = new ObserverInfo(Context.ConnectionId, date);
            if (TimetableObservers.ContainsKey(doctorId))
            {
                TimetableObservers[doctorId].Remove(observerInfo);
            }
        }

        public void NotifyTimetableChanged(int doctorId, DateTime date)
        {
            ICollection<ObserverInfo> observers;

            if (TimetableObservers.TryGetValue(doctorId, out observers))
            {
                var sessions = observers.Where(info => info.Date.Date == date.Date).Select(observer => observer.ObserverIdentifier);

                try
                {
                    var clients = this.InjectedClients.Clients(sessions.ToList());
                    clients.TimetableChanged(doctorId, date);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
