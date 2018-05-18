namespace Hms.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    using Microsoft.AspNet.SignalR.Client;

    public class NotificationService : INotificationService
    {
        private HubConnection connection;

        private IHubProxy hubProxy;

        public Dictionary<ObserverInfo, Func<Task>> observerInfos = new Dictionary<ObserverInfo, Func<Task>>();

        public NotificationService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public async Task ConnectAsync()
        {
            this.connection = new HubConnection($"{this.RequestCoordinator.Host}signalr");
            this.hubProxy = this.connection.CreateHubProxy("NotificationHub");

            this.connection.TraceLevel = TraceLevels.All;
            this.connection.TraceWriter = Console.Out;

            ServicePointManager.DefaultConnectionLimit = 10;

            await this.connection.Start();

            await this.hubProxy.Invoke("ConnectDoctor", this.RequestCoordinator.ClientState.Identifier);

            this.hubProxy.On("TimetableChanged", async (int doctorId, DateTime date) => await this.TimetableChanged(doctorId, date));
        }

        public async Task SubscribeAsync(int doctorId, DateTime date, Func<Task> callback)
        {
            await this.hubProxy.Invoke("ConnectToTimetable", doctorId, date);

            this.observerInfos.Add(new ObserverInfo(doctorId.ToString(), date.Date), callback);
        }

        public async Task UnsubscribeAsync(int doctorId, DateTime date)
        {
            await this.hubProxy.Invoke("DisconnectFromTimetable", doctorId, date);

            this.observerInfos.Remove(new ObserverInfo(doctorId.ToString(), date.Date));
        }

        public async Task TimetableChanged(int doctorId, DateTime date)
        {
            var info = new ObserverInfo(doctorId.ToString(), date);
            var callbacks = this.observerInfos.Where(kvp => kvp.Key.Equals(info)).Select(kvp => kvp.Value);

            foreach (var callback in callbacks)
            {
                await callback();
            }
        }
    }
}
