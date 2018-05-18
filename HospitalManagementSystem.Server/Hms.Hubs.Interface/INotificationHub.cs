namespace Hms.Hubs.Interface
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationService
    {
        Task NotifyTimetableChangedAsync(int doctorId, DateTime date);
    }
}
