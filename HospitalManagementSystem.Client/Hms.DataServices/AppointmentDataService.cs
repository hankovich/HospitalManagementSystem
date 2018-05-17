namespace Hms.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class AppointmentDataService : IAppointmentDataService
    {
        public AppointmentDataService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public async Task<IEnumerable<CalendarItem>> GetAppointmentsAsync(int doctorId, DateTime date)
        {
            var dateString = $"{date.Year}/{date.Month}/{date.Day}"; //date.ToString("yyyy/MM/dd");
            var response = await this.RequestCoordinator.SendAsync<IEnumerable<CalendarItem>>(HttpMethod.Get, $"api/appointment/{doctorId}?date={date}");

            return response.Content;
        }

        public async Task<int> ScheduleAppointmentAsync(int userId, int doctorId, DateTime selectedDate, DateTime startDate)
        {
            var startDateTime = selectedDate.Date + startDate.TimeOfDay;

            var calendarItem = new CalendarItem
            {
                Owner = new User { Id = userId },
                AssociatedUsers = new List<User> { new User { Id = doctorId } },
                StartDate = startDateTime,
                EndDate = startDateTime + TimeSpan.FromMinutes(15)
            };

            var response = await this.RequestCoordinator.SendAsync<int>(HttpMethod.Post, "api/appointment", calendarItem);

            return response.Content;
        }

        public Task CancelAppointmentAsync(int appointmentId)
        {
            return this.RequestCoordinator.SendAsync<int>(HttpMethod.Delete, "api/appointment", appointmentId);
        }
    }
}
