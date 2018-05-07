namespace Hms.UI.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;
    using Hms.UI.Wrappers;

    using Prism.Events;

    public class DoctorViewModel : ViewModelBase
    {
        private ProfileWrapper profile;

        private Doctor doctor;

        public DoctorViewModel(
            int doctorId,
            object parentViewModel,
            IDoctorDataService doctorDataService,
            IProfileDataService profileDataService,
            IEventAggregator eventAggregator)
        {
            this.DoctorId = doctorId;
            this.DoctorDataService = doctorDataService;
            this.ProfileDataService = profileDataService;
            this.EventAggregator = eventAggregator;

            this.LoadedCommand = AsyncCommand.Create(this.OnLoadedAsync);
            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        public Doctor Doctor
        {
            get
            {
                return this.doctor;
            }

            set
            {
                this.doctor = value;
                this.OnPropertyChanged();
            }
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

        public int DoctorId { get; }

        public IDoctorDataService DoctorDataService { get; }

        public IProfileDataService ProfileDataService { get; }

        public IEventAggregator EventAggregator { get; }

        public ICommand LoadedCommand { get; }

        public ICommand BackCommand { get; }

        private async Task OnLoadedAsync()
        {
            this.Doctor = await this.DoctorDataService.GetDoctorAsync(this.DoctorId);
            var profileModel = await this.ProfileDataService.GetProfileAsync(this.Doctor.Id);
            this.Profile = new ProfileWrapper(profileModel);
        }
    }
}