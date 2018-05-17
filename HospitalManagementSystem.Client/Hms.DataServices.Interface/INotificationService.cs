namespace Hms.DataServices.Interface
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationService
    {
        Task ConnectAsync();

        Task SubscribeAsync(int doctorId, DateTime date, Func<Task> callback);

        Task UnsubscribeAsync(int doctorId, DateTime date);
    }
}
