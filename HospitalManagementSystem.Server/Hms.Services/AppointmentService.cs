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
        public AppointmentService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, INotificationHub notificationHub)
        {
            this.AppointmentRepository = appointmentRepository;
            this.DoctorRepository = doctorRepository;
            this.NotificationHub = notificationHub;
        }

        public IAppointmentRepository AppointmentRepository { get; }

        public IDoctorRepository DoctorRepository { get; }

        public INotificationHub NotificationHub { get; }

        public Task<IEnumerable<CalendarItem>> GetAppointmentsAsync(int doctorId, DateTime date, int userId)
        {
            return this.AppointmentRepository.GetAppointmentsAsync(doctorId, date, userId);
        }

        public async Task<int> ScheduleAppointmentAsync(int userId, CalendarItem calendarItem)
        {
            int appointmentId = await this.AppointmentRepository.ScheduleAppointmentAsync(userId, calendarItem);

            List<int> participants = calendarItem.AssociatedUsers.Select(u => u.Id).ToList();
            participants.Add(userId);

            var doctorParticipants = await this.DoctorRepository.GetDoctorIdsAsync(participants);

            foreach (var doctorId in doctorParticipants)
            {
                this.NotificationHub.NotifyTimetableChanged(doctorId, calendarItem.StartDate.Date);
            }
            
            return appointmentId;
        }

        public Task CancelAppointmentAsync(int userId, int appointmentId)
        {
            return this.AppointmentRepository.CancelAppointmentAsync(userId, appointmentId);
        }
    }
}
