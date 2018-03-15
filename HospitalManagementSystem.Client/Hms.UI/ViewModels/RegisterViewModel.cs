namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Hms.Services.Interface;
    using Hms.UI.Annotations;
    using Hms.UI.Infrastructure.Commands;

    using MahApps.Metro.Controls.Dialogs;

    public class RegisterViewModel : INotifyPropertyChanged
    {
        public RegisterViewModel(IAccountService accountService, IDialogCoordinator dialogCoordinator)
        {
            this.AccountService = accountService;
            this.DialogCoordinator = dialogCoordinator;

            this.CheckPasswordsCommand = AsyncCommand.Create<IEnumerable<PasswordBox>>(this.ValidateRegistrationInfoAsync);

            this.RegisterCommand = AsyncCommand.Create<IEnumerable<PasswordBox>>(this.RegisterAsync);
        }

        public ICommand CheckPasswordsCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public string Login { get; set; }

        public IAccountService AccountService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        private async Task ValidateRegistrationInfoAsync(IEnumerable<PasswordBox> boxes)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Login))
                {
                    throw new ArgumentException("Login must not be empty");
                }

                var boxesArray = boxes.ToArray();

                var pass = boxesArray[0];
                var confirm = boxesArray[1];

                if (pass.Password.Length < 6)
                {
                    throw new ArgumentException("Length of password must be at least 6");
                }

                if (pass.Password != confirm.Password)
                {
                    throw new ArgumentException("Passwords must match");
                }
            }
            catch (Exception e)
            {
                await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                throw;
            }
        }

        private async Task RegisterAsync(IEnumerable<PasswordBox> boxes)
        {
            try
            {
                string password = boxes.First().Password;
                await this.AccountService.RegisterAsync(this.Login, password);
                await this.AccountService.LoginAsync(this.Login, password);
            }
            catch (Exception e)
            {
                await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
