namespace Hms.Services
{
    using System;
    using System.Threading.Tasks;

    using Hms.Repositories.Interface;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }

            this.UserRepository = userRepository;
            this.UserRoleRepository = userRoleRepository;
        }

        public IUserRepository UserRepository { get; }

        public IUserRoleRepository UserRoleRepository { get; }

        public async Task AddUserAsync(string username, string password)
        {
            await this.UserRepository.AddUserAsync(username, password);
            await this.UserRoleRepository.AddRoleToUserAsync(username, nameof(Role.Patient));
        }

        public async Task<Interface.User> GetUserAsync(string username, string password)
        {
            var user = await this.UserRepository.GetUserAsync(username, password);

            return new Interface.User { Login = user.Username };
        }

        public async Task<bool> CheckCredentials(string username, string password)
        {
            try
            {
                await this.UserRepository.GetUserAsync(username, password);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
