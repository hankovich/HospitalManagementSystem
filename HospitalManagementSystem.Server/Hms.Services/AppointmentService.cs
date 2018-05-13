namespace Hms.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class AppointmentService : IAppointmentService
    {
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            this.AppointmentRepository = appointmentRepository;
        }

        public IAppointmentRepository AppointmentRepository { get; }

        public Task<IEnumerable<CalendarItem>> GetAppointmentsAsync(int doctorId, DateTime date, int userId)
        {
            return this.AppointmentRepository.GetAppointmentsAsync(doctorId, date, userId);
        }

        public Task<int> ScheduleAppointmentAsync(int userId, CalendarItem calendarItem)
        {
            return this.AppointmentRepository.ScheduleAppointmentAsync(userId, calendarItem);
        }

        public Task CancelAppointmentAsync(int userId, int appointmentId)
        {
            return this.AppointmentRepository.CancelAppointmentAsync(userId, appointmentId);
        }
    }
}
