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

        public DoctorViewModel(
            Doctor doctor,
            object parentViewModel,
            IProfileDataService profileDataService,
            IEventAggregator eventAggregator)
        {
            this.Doctor = doctor;
            this.ProfileDataService = profileDataService;
            this.EventAggregator = eventAggregator;

            this.LoadedCommand = AsyncCommand.Create(this.OnLoadedAsync);
            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        public Doctor Doctor { get; }

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

        public IProfileDataService ProfileDataService { get; }

        public IEventAggregator EventAggregator { get; }

        public ICommand LoadedCommand { get; }

        public ICommand BackCommand { get; }

        private async Task OnLoadedAsync()
        {
            var profileModel = await this.ProfileDataService.GetProfileAsync(this.Doctor.Id);
            this.Profile = new ProfileWrapper(profileModel);
        }
    }
}