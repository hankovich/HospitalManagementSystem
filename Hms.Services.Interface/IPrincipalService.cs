namespace Hms.Services.Interface
{
    using System.Security.Principal;

    using Hms.Common;

    public interface IPrincipalService
    {
        IPrincipal ModelToPrincipal(PrincipalModel model);

        PrincipalModel PrincipalToModel(IPrincipal principal);
    }
}
