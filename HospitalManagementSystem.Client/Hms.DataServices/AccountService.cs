namespace Hms.DataServices
{
    using System.Threading.Tasks;

    using Hms.DataServices.Interface;

    public class AccountService : IAccountService
    {
        public AccountService(IRequestCoordinator client)
        {
            if (client == null)
            {
                throw new System.ArgumentNullException(nameof(client));
            }

            this.Client = client;
        }

        public IRequestCoordinator Client { get; }

        public async Task LoginAsync(string login, string password)
        {
            await this.Client.LoginAsync(login, password);
        }

        public async Task RegisterAsync(string login, string password)
        {
            await this.Client.RegisterAsync(login, password);
        }

        public async Task LogoutAsync()
        {
            await this.Client.LogoutAsync();
        }
    }
}
