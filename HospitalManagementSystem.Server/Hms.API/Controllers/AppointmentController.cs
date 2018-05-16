namespace Hms.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    [RoutePrefix("api/appointment")]
    public class AppointmentController : ApiController
    {
        public AppointmentController(IAppointmentService appointmentService, IUserService userService)
        {
            this.AppointmentService = appointmentService;
            this.UserService = userService;
        }

        public IAppointmentService AppointmentService { get; }

        public IUserService UserService { get; }

        [HttpGet, Route("{doctorId}/{*date:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        public async Task<IHttpActionResult> Get(int doctorId, DateTime date)
        {
            try
            {
                int userId = await this.UserService.GetUserIdByLoginAsync(this.User.Identity.Name);

                return this.Ok(await this.AppointmentService.GetAppointmentsAsync(doctorId, date, userId));
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [HttpPost, Route]
        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        public async Task<IHttpActionResult> Schedule([FromBody]CalendarItem calendarItem)
        {
            try
            {
                int userId = await this.UserService.GetUserIdByLoginAsync(this.User.Identity.Name);

                return this.Ok(
                    await this.AppointmentService.ScheduleAppointmentAsync(userId, calendarItem));
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [HttpDelete, Route]
        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        public async Task<IHttpActionResult> Cancel([FromBody]int appointmentId)
        {
            try
            {
                int userId = await this.UserService.GetUserIdByLoginAsync(this.User.Identity.Name);

                await this.AppointmentService.CancelAppointmentAsync(userId, appointmentId);
                return this.Ok();
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}