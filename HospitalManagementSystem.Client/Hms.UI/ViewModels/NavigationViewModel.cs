namespace Hms.UI.ViewModels
{
    using System.Windows.Input;

    using Hms.UI.Infrastructure.Commands;

    using Ninject;

    public class NavigationViewModel : ViewModelBase
    {
        private object selectedViewModel;

        public NavigationViewModel()
        {
            this.MenuCommand = new RelayCommand(this.OpenMenu);
            this.ProfileCommand = new RelayCommand(this.OpenProfile);
            this.MainCommand = new RelayCommand(this.OpenMain);
            this.LoginCommand = new RelayCommand(this.OpenLogin);
            this.RegisterCommand = new RelayCommand(this.OpenRegister);
            this.CreateProfileCommand = new RelayCommand(this.OpenCreateProfile);

            this.OpenLogin();
        }

        public ICommand MenuCommand { get; }

        public ICommand ProfileCommand { get; }

        public ICommand MainCommand { get; }

        public ICommand LoginCommand { get; }

        public ICommand RegisterCommand { get; }

        public ICommand CreateProfileCommand { get; }

        public object SelectedViewModel
        {
            get
            {
                return this.selectedViewModel;
            }

            set
            {
                if (this.selectedViewModel != value)
                {
                    this.selectedViewModel = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private void OpenLogin()
        {
            this.SelectedViewModel = App.Kernel.Get<LoginViewModel>();
        }

        private void OpenMenu()
        {
            this.SelectedViewModel = App.Kernel.Get<MenuViewModel>();
        }

        private void OpenProfile()
        {
            this.SelectedViewModel = App.Kernel.Get<ProfileViewModel>();
        }

        private void OpenMain()
        {
            this.SelectedViewModel = App.Kernel.Get<MainViewModel>();
        }

        private void OpenRegister()
        {
            this.SelectedViewModel = App.Kernel.Get<RegisterViewModel>();
        }

        private void OpenCreateProfile()
        {
            this.SelectedViewModel = App.Kernel.Get<CreateProfileViewModel>();
        }
    }
}