namespace Hms.UI.ViewModels
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;

    using MahApps.Metro.Controls.Dialogs;

    public class LoginViewModel : ViewModelBase
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
                    throw;
                }
            });
        }

        public IAccountService AccountService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public ICommand LoginCommand { get; }

        public string Login { get; set; }
    }
}
