namespace Hms.Repositories.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserRoleRepository
    {
        Task<IEnumerable<string>> GetUserRolesAsync(string login);

        Task AddRoleToUserAsync(string login, string rolename);
    }
}
