namespace Hms.UI.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using Hms.UI.Annotations;
    using Hms.UI.Infrastructure.Commands;

    using Ninject;

    public class NavigationViewModel : INotifyPropertyChanged
    {
        public ICommand MenuCommand { get; set; }

        public ICommand ProfileCommand { get; set; }

        public ICommand MainCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        private object selectedViewModel;

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

        public NavigationViewModel()
        { 
            this.MenuCommand = new RelayCommand(this.OpenMenu);
            this.ProfileCommand = new RelayCommand(this.OpenProfile);
            this.MainCommand = new RelayCommand(this.OpenMain);
            this.LoginCommand = new RelayCommand(this.OpenLogin);

            this.LoginCommand.Execute(null);
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}