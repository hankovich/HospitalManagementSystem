namespace Hms.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    using Hms.Common.Interface.Models;
    using Hms.Services.Interface;

    public class PrincipalService : IPrincipalService
    {
        public IPrincipal ModelToPrincipal(PrincipalModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new ClaimsPrincipal(
                new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.UserData, Convert.ToBase64String(model.RoundKey)),
                            new Claim(ClaimTypes.Name, model.Login)
                        })
                });
        }

        public PrincipalModel PrincipalToModel(IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claimsPrincipal = principal as ClaimsPrincipal;

            if (claimsPrincipal == null)
            {
                throw new NotSupportedException(
                    $"{nameof(principal)} is not ClaimsPrincipal. {principal.GetType().Name} is not supported");
            }

            return new PrincipalModel
            {
                Login = claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypes.Name).Value,
                RoundKey = Convert.FromBase64String(
                    claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypes.UserData).Value)
            };
        }
    }
}