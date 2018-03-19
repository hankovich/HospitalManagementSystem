namespace Hms.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using AuthorizeAttribute = Hms.API.Attributes.AuthorizeAttribute;

    [RoutePrefix("api/profile")]
    public class ProfileController : ApiController
    {
        public ProfileController(IProfileService profileService, IUserService userService)
        {
            if (profileService == null)
            {
                throw new ArgumentNullException(nameof(profileService));
            }

            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            this.ProfileService = profileService;
            this.UserService = userService;
        }

        public IProfileService ProfileService { get; }

        public IUserService UserService { get; }

        [Encrypted, Authorize(Roles = Role.Patient)]
        [HttpGet, Route]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                int id = await this.UserService.GetUserIdByLoginAsync(this.User.Identity.Name);
                Profile profile = await this.ProfileService.GetProfileAsync(id);

                return this.Ok(profile);
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [Encrypted, Authorize(Roles = Role.Patient)]
        [HttpGet, Route("{userId}")]
        public async Task<IHttpActionResult> Get(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(userId));
                }

                Profile profile = await this.ProfileService.GetProfileAsync(userId);

                return this.Ok(profile);
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [Encrypted, Authorize(Roles = Role.Patient)]
        [HttpPut, Route]
        public async Task<IHttpActionResult> Put([FromBody] Profile profile)
        {
            try
            {
                int userId = await this.UserService.GetUserIdByLoginAsync(this.User.Identity.Name);

                if (profile.UserId != userId)
                {
                    return this.BadRequest("You can modify only your profile");
                }

                await this.ProfileService.InsertOrUpdateProfileAsync(profile);

                return this.Ok(profile.Id);
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}