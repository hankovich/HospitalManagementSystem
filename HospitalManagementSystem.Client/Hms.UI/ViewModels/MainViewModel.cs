namespace Hms.UI.ViewModels
{
    using System;
    using System.Reflection;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;

    using Ninject;

    using Prism.Events;

    public class MainViewModel : ViewModelBase
    {
        public IEventAggregator EventAggregator { get; }

        private object selectedViewModel;

        public MainViewModel(IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
            this.CardCommand = new RelayCommand(this.OpenCard);

            this.OpenCard();

            eventAggregator.GetEvent<OpenMenuItemEvent>().Subscribe(this.OnOpenMenuItem);

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += (sender, args) => this.OnPropertyChanged(nameof(this.Time));
            timer.Start();
        }

        public string Time => DateTime.Now.ToString("HH:mm:ss");

        public ICommand CardCommand { get; }

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

        private void OpenCard()
        {
            this.SelectedViewModel = App.Kernel.Get<MedicalCardViewModel>();
        }

        private void OnOpenMenuItem(string viewModelName)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var type = executingAssembly.GetType($"Hms.UI.ViewModels.{viewModelName}");
            this.SelectedViewModel = App.Kernel.Get(type);
        }
    }
}
