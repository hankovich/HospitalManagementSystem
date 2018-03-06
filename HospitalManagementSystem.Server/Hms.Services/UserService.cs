namespace Hms.Services
{
    using System;
    using System.Threading.Tasks;

    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }

            this.UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; }

        public async Task AddUserAsync(string username, string password)
        {
            await this.UserRepository.AddUserAsync(username, password);
        }

        public async Task<Interface.User> GetUserAsync(string username, string password)
        {
            var user = await this.UserRepository.GetUserAsync(username, password);

            return new Interface.User { Login = user.Username };
        }
    }
}
