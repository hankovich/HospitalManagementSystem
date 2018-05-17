namespace Hms.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            await this.connection.Start();

            await this.hubProxy.Invoke("ConnectDoctor", this.RequestCoordinator.ClientState.Identifier);
        }

        public async Task SubscribeAsync(int doctorId, DateTime date, Func<Task> callback)
        {
            await this.hubProxy.Invoke("ConnectToTimetable", doctorId, date);

            this.observerInfos.Add(new ObserverInfo(doctorId.ToString(), date), callback);
        }

        public async Task UnsubscribeAsync(int doctorId, DateTime date)
        {
            await this.hubProxy.Invoke("DisconnectFromTimetable", doctorId, date);

            this.observerInfos.Remove(new ObserverInfo(doctorId.ToString(), date));
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

    public struct ObserverInfo : IEquatable<ObserverInfo>
    {
        public string ObserverIdentifier { get; }

        public DateTime Date { get; }

        public ObserverInfo(string observerIdentifier, DateTime date)
        {
            this.ObserverIdentifier = observerIdentifier;
            this.Date = date;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.ObserverIdentifier != null ? this.ObserverIdentifier.GetHashCode() : 0) * 397) ^ this.Date.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(ObserverInfo other)
        {
            return string.Equals(this.ObserverIdentifier, other.ObserverIdentifier) && this.Date.Equals(other.Date);
        }
    }
}
