namespace Hms.UI.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Hms.Services.Interface;
    using Hms.UI.Annotations;
    using Hms.UI.Infrastructure.Commands;

    using MahApps.Metro.Controls.Dialogs;

    public class LoginViewModel : INotifyPropertyChanged
    {
        public LoginViewModel(IAccountService accountService, IDialogCoordinator dialogCoordinator)
        {
            this.AccountService = accountService;
            this.DialogCoordinator = dialogCoordinator;

            this.LoginCommand = AsyncCommand.Create<PasswordBox>(async passwordBox =>
            {
                try
                {
                    await this.AccountService.LoginAsync(this.Login, passwordBox.Password);
                }
                catch (Exception e)
                {
                    await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                }
            });
        }

        public IAccountService AccountService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public string Login { get; set; }

        public ICommand LoginCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
