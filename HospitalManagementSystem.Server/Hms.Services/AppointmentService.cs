namespace Hms.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Hubs.Interface;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class AppointmentService : IAppointmentService
    {
        public AppointmentService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, INotificationService notificationHub)
        {
            this.AppointmentRepository = appointmentRepository;
            this.DoctorRepository = doctorRepository;
            this.NotificationHub = notificationHub;
        }

        public IAppointmentRepository AppointmentRepository { get; }

        public IDoctorRepository DoctorRepository { get; }

        public INotificationService NotificationHub { get; }

        public Task<IEnumerable<CalendarItem>> GetAppointmentsAsync(int doctorId, DateTime date, int userId)
        {
            return this.AppointmentRepository.GetAppointmentsAsync(doctorId, date, userId);
        }

        public async Task<int> ScheduleAppointmentAsync(int userId, CalendarItem calendarItem)
        {
            int appointmentId = await this.AppointmentRepository.ScheduleAppointmentAsync(userId, calendarItem);

            calendarItem.Owner = new User { Id = userId };
            await this.NotifyDoctorsChangedAsync(calendarItem);

            return appointmentId;
        }

        public async Task CancelAppointmentAsync(int userId, int appointmentId)
        {
            var calendarItem = await this.AppointmentRepository.GetAppointmentAsync(appointmentId);

            await this.AppointmentRepository.CancelAppointmentAsync(userId, appointmentId);

            await this.NotifyDoctorsChangedAsync(calendarItem);
        }

        private async Task NotifyDoctorsChangedAsync(CalendarItem calendarItem)
        {
            List<int> participants = calendarItem.AssociatedUsers.Select(u => u.Id).ToList();
            participants.Add(calendarItem.Owner.Id);

            var doctorParticipants = await this.DoctorRepository.GetDoctorIdsAsync(participants);

            foreach (var doctorId in doctorParticipants)
            {
                await this.NotificationHub.NotifyTimetableChangedAsync(doctorId, calendarItem.StartDate.Date);
            }
        }
    }
}
