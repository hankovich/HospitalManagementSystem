namespace Hms.Hubs.Interface
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationHub
    {
        Task NotifyTimetableChangedAsync(int doctorId, DateTime date);
    }
}
