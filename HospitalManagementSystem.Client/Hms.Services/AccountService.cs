﻿namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Services.Interface;

    public class AccountService : IAccountService
    {
        public AccountService(IClient client)
        {
            if (client == null)
            {
                throw new System.ArgumentNullException(nameof(client));
            }

            this.Client = client;
        }

        public IClient Client { get; }

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