namespace Hms.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hms.Hubs.Interface;
    using Hms.Services.Interface;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    public class NotificationHub : Hub, INotificationService
    {
        private static readonly ConcurrentDictionary<string, string> ConnectionToGadgetMappings =
            new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<int, ICollection<ObserverInfo>> TimetableObservers =
            new ConcurrentDictionary<int, ICollection<ObserverInfo>>();

        private static IHubCallerConnectionContext<dynamic> ConnectionContext;

        private static readonly SemaphoreSlim DisconnectedSemaphore = new SemaphoreSlim(1, 1);

        public NotificationHub(IUserSessionService userSessionService)
        {
            this.UserSessionService = userSessionService;
        }

        public IUserSessionService UserSessionService { get; }

        public override async Task OnDisconnected(bool stopCalled)
        {
            string result = string.Empty;
            ConnectionToGadgetMappings.TryRemove(this.Context.ConnectionId, out result);

            await DisconnectedSemaphore.WaitAsync();

            try
            {
                var disconectedUserSubscribtions = TimetableObservers
                    .Where(kvp => kvp.Value.Count(oi => oi.ObserverIdentifier == this.Context.ConnectionId) > 0)
                    .Select(kvp => new { DoctorId = kvp.Key, Elements = kvp.Value });

                foreach (var subscribtion in disconectedUserSubscribtions)
                {
                    foreach (var connectionInfo in subscribtion.Elements)
                    {
                        TimetableObservers[subscribtion.DoctorId].Remove(connectionInfo);
                    }
                }
            }
            finally
            {
                DisconnectedSemaphore.Release();
            }

            await base.OnDisconnected(stopCalled);
        }

        public void ConnectDoctor(string identifier)
        {
            ConnectionToGadgetMappings.AddOrUpdate(this.Context.ConnectionId, s => identifier, (s, s1) => identifier);
        }

        public void ConnectToTimetable(int doctorId, DateTime date)
        {
            var observerInfo = new ObserverInfo(this.Context.ConnectionId, date);

            TimetableObservers.AddOrUpdate(
                doctorId,
                id => new List<ObserverInfo> { observerInfo },
                (id, pairs) =>
                {
                    pairs.Add(observerInfo);
                    return pairs;
                });

            ConnectionContext = this.Clients;
        }

        public void DisconnectFromTimetable(int doctorId, DateTime date)
        {
            var observerInfo = new ObserverInfo(Context.ConnectionId, date);
            if (TimetableObservers.ContainsKey(doctorId))
            {
                TimetableObservers[doctorId].Remove(observerInfo);
            }

            ConnectionContext = this.Clients;
        }

        public async Task NotifyTimetableChangedAsync(int doctorId, DateTime date)
        {
            ICollection<ObserverInfo> observers;
            
            if (TimetableObservers.TryGetValue(doctorId, out observers))
            {
                var sessions = observers.Where(info => info.Date.Date == date.Date)
                    .Select(observer => observer.ObserverIdentifier).ToList();

                await ConnectionContext.Clients(sessions).TimetableChanged(doctorId, date);
            }
        }
    }
}
