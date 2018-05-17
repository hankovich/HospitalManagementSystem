namespace Hms.Hubs.Interface
{
    using System;

    public interface INotificationHub
    {
        void NotifyTimetableChanged(int doctorId, DateTime date);
    }
}
