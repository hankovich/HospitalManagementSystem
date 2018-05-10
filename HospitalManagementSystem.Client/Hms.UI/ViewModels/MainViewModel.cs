namespace Hms.UI.ViewModels
{
    using System;
    using System.Reflection;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;

    using Ninject;
    using Ninject.Parameters;

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
            eventAggregator.GetEvent<OpenRecordEvent>().Subscribe(this.OnOpenRecord);
            eventAggregator.GetEvent<NavigationEvent>().Subscribe(this.OnNavigation);
            eventAggregator.GetEvent<OpenDoctorEvent>().Subscribe(this.OnOpenDoctor);
            eventAggregator.GetEvent<OpenSpecializationDoctorsEvent>().Subscribe(this.OnOpenSpecializationDoctors);

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

        private void OnOpenRecord(OpenRecordEventArgs args)
        {
            var recordId = new ConstructorArgument("recordId", args.RecordId);
            var parentViewModel = new ConstructorArgument("parentViewModel", args.ParentViewModel);
            this.SelectedViewModel = App.Kernel.Get<MedicalCardRecordViewModel>(recordId, parentViewModel);
        }

        private void OnOpenMenuItem(string viewModelName)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var type = executingAssembly.GetType($"Hms.UI.ViewModels.{viewModelName}");

            if (type == this.SelectedViewModel.GetType())
            {
                return;
            }

            this.SelectedViewModel = App.Kernel.Get(type);
        }

        private void OnNavigation(object obj)
        {
            this.SelectedViewModel = obj;
        }

        private void OnOpenDoctor(OpenDoctorEventArgs args)
        {
            var recordId = new ConstructorArgument("doctorId", args.DoctorId);
            var parentViewModel = new ConstructorArgument("parentViewModel", args.ParentViewModel);
            this.SelectedViewModel = App.Kernel.Get<DoctorViewModel>(recordId, parentViewModel);
        }

        private void OnOpenSpecializationDoctors(OpenSpecializationDoctorsArgs args)
        {
            var polyclinicId = new ConstructorArgument("polyclinicId", args.PolyclinicId);
            var specializationId = new ConstructorArgument("specializationId", args.SpecializationId);
            var parentViewModel = new ConstructorArgument("parentViewModel", args.ParentViewModel);
            this.SelectedViewModel = App.Kernel.Get<SpecializationDoctorsViewModel>(polyclinicId, specializationId, parentViewModel);
        }
    }
}
