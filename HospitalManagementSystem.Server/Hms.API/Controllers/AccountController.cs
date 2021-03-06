﻿namespace Hms.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.API.Infrastructure;
    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Models;
    using Hms.Services.Interface;

    [Route("api/account")]
    public class AccountController : ApiController
    {
        public IUserService UserService { get; set; }

        public AccountController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            this.UserService = userService;
        }

        [HttpPost, Encrypted]
        public async Task<IHttpActionResult> Post([FromBody] LoginModel model)
        {
            if (model?.Login == null || model.Password == null)
            {
                return this.BadRequest("Invalid arguments");
            }

            try
            {
                await this.UserService.AddUserAsync(model.Login, model.Password);
                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpPut, Encrypted]
        public async Task<IHttpActionResult> CheckUser([FromBody] LoginModel model)
        {
            if (model?.Login == null || model.Password == null)
            {
                return this.BadRequest("Invalid arguments");
            }

            try
            {
                User user = await this.UserService.GetUserAsync(model.Login, model.Password);
                return this.Ok(user.Id);
            }
            catch
            {
                return new HttpActionResult(this, HttpStatusCode.Unauthorized, "Invalid credentials");
            }
        }
    }
}
