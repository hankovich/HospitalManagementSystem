namespace Hms.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class AppointmentRepository : IAppointmentRepository
    {
        public AppointmentRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public async Task<IEnumerable<CalendarItem>> GetAppointmentsAsync(int doctorId, DateTime date, int userId)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT DISTINCT CI.[Id], [StartDateUtc] AS [StartDate], [EndDateUtc] AS [EndDate], CI.[UserId] AS [Id]
                    FROM [CalendarItem] CI
                    LEFT JOIN [CalendarItemAssociatedUser] CIAU
                    ON CI.[Id] = CIAU.[CalendarItemId]
                    WHERE CIAU.[UserId] = @doctorId AND CONVERT(DATE, CI.[StartDateUtc]) = CONVERT(DATE, @date)";

                    var items =
                        await
                        connection.QueryAsync<CalendarItem, int, CalendarItem>(
                            command,
                            (item, ownerId) =>
                            {
                                item.Owner = new User();

                                if (ownerId == userId)
                                {
                                    item.Owner.Id = userId;
                                }

                                item.AssociatedUsers = new List<User> { new User { Id = doctorId } };
                                return item;
                            },
                            new { doctorId, date });

                    return items;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> ScheduleAppointmentAsync(int userId, CalendarItem calendarItem)
        {
            try
            {
                if (calendarItem.Owner.Id != userId)
                {
                    throw new ArgumentException("You can't schedule appointement");    
                }

                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    INSERT INTO [CalendarItem]([UserId], [StartDateUtc], [EndDateUtc], [Info]) OUTPUT INSERTED.ID VALUES(@userId, @StartDate, @EndDate, @Info)"; // TODO: user wins / database wins

                    if (calendarItem.Info == null)
                    {
                        calendarItem.Info = string.Empty;
                    }

                    int appointmentId = await connection.QueryFirstOrDefaultAsync<int>(command, new { userId, calendarItem.StartDate, calendarItem.EndDate, calendarItem.Info });

                    foreach (var user in calendarItem.AssociatedUsers)
                    {
                        var insertAssociatedUserCommand = @"
                        INSERT INTO [CalendarItemAssociatedUser]([CalendarItemId], [UserId]) VALUES(@appointmentId, @Id)";

                        await
                            connection.ExecuteAsync(
                                insertAssociatedUserCommand,
                                new { appointmentId, user.Id });
                    }

                    return appointmentId;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task CancelAppointmentAsync(int userId, int appointmentId)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    DELETE 
                    FROM [CalendarItem]
                    WHERE [UserId] = @userId AND [Id] = @appointmentId

                    DELETE FROM [CalendarItemAssociatedUser]
                    WHERE [UserId] = @userId AND [CalendarItemId] = @appointmentId";

                    await connection.ExecuteAsync(command, new { userId, appointmentId });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
