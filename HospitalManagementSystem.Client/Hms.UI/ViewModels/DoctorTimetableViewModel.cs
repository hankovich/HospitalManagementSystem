namespace Hms.UI.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;
    using Hms.UI.Wrappers;

    using Prism.Events;

    public class DoctorTimetableViewModel : ViewModelBase
    {
        private ProfileWrapper profile;

        public DoctorTimetableViewModel(int doctorId, object parentViewModel, IProfileDataService profileDataService, IEventAggregator eventAggregator)
        {
            this.DoctorId = doctorId;
            this.ParentViewModel = parentViewModel;
            this.ProfileDataService = profileDataService;
            this.EventAggregator = eventAggregator;

            this.LoadedCommand = AsyncCommand.Create(this.OnLoadedAsync);

            this.OpenDoctorCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<OpenDoctorEvent>().Publish(
                    new OpenDoctorEventArgs { DoctorId = this.DoctorId, ParentViewModel = this }));
            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        private async Task OnLoadedAsync()
        {
            var profileModel = await this.ProfileDataService.GetProfileAsync(this.DoctorId);
            this.Profile = new ProfileWrapper(profileModel);
        }

        public ProfileWrapper Profile
        {
            get
            {
                return this.profile;
            }

            set
            {
                this.profile = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand LoadedCommand { get; }

        public ICommand OpenDoctorCommand { get; }

        public ICommand BackCommand { get; }

        public int DoctorId { get; }

        public object ParentViewModel { get; }

        public IProfileDataService ProfileDataService { get; }

        public IEventAggregator EventAggregator { get; }
    }
}
