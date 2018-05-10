namespace Hms.UI.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;

    using Prism.Events;

    public class SpecializationDoctorsViewModel : ViewModelBase
    {
        private int polyclinicId;

        private int specializationId;

        private MedicalSpecialization specialization;

        private Polyclinic polyclinic;

        public SpecializationDoctorsViewModel(
            int polyclinicId,
            int specializationId,
            object parentViewModel,
            IMedicalSpecializationDataService specializationDataService,
            IPolyclinicDataService polyclinicDataService,
            IEventAggregator eventAggregator)
        {
            this.PolyclinicId = polyclinicId;
            this.SpecializationId = specializationId;
            this.SpecializationDataService = specializationDataService;
            this.PolyclinicDataService = polyclinicDataService;
            this.EventAggregator = eventAggregator;

            this.LoadedCommand = AsyncCommand.Create(this.OnLoaded);
            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        public ICommand LoadedCommand { get; }

        private async Task OnLoaded()
        {
            this.Specialization =
                await this.SpecializationDataService.GetMedicalSpecializationAsync(this.SpecializationId);

            this.Polyclinic = await this.PolyclinicDataService.GetPolyclinicAsync(this.PolyclinicId);
        }

        public Polyclinic Polyclinic
        {
            get
            {
                return this.polyclinic;
            }

            set
            {
                this.polyclinic = value;
                this.OnPropertyChanged();
            }
        }

        public MedicalSpecialization Specialization
        {
            get
            {
                return this.specialization;
            }

            set
            {
                this.specialization = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; }

        public int PolyclinicId
        {
            get
            {
                return this.polyclinicId;
            }

            set
            {
                this.polyclinicId = value;
                this.OnPropertyChanged();
            }
        }

        public int SpecializationId
        {
            get
            {
                return this.specializationId;
            }

            set
            {
                this.specializationId = value;
                this.OnPropertyChanged();
            }
        }

        public IMedicalSpecializationDataService SpecializationDataService { get; }

        public IPolyclinicDataService PolyclinicDataService { get; set; }

        public IEventAggregator EventAggregator { get; }
    }
}
