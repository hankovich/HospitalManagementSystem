namespace Hms.DataServices.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IAppointmentDataService
    {
        Task<IEnumerable<CalendarItem>> GetAppointmentsAsync(int doctorId, DateTime date);

        Task<int> ScheduleAppointmentAsync(int userId, int doctorId, DateTime selectedDate, DateTime startDate);

        Task CancelAppointmentAsync(int appointmentId);
    }
}
