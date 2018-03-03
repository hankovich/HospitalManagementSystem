namespace Hms.Services.Interface
{
    using System.Security.Principal;

    using Hms.Common.Interface.Models;

    public interface IPrincipalService
    {
        IPrincipal ModelToPrincipal(PrincipalModel model);

        PrincipalModel PrincipalToModel(IPrincipal principal);
    }
}
