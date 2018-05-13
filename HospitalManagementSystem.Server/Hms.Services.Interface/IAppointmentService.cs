namespace Hms.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IAppointmentService
    {
        Task<IEnumerable<CalendarItem>> GetAppointmentsAsync(int doctorId, DateTime date, int userId);

        Task<int> ScheduleAppointmentAsync(int userId, CalendarItem calendarItem);

        Task CancelAppointmentAsync(int userId, int appointmentId);
    }
}
