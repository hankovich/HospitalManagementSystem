namespace Hms.UI.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Controls.PagingControl;
    using Hms.UI.Infrastructure.Events;

    using Prism.Events;

    public class SpecializationDoctorsViewModel : ViewModelBase
    {
        private int polyclinicId;

        private int specializationId;

        private MedicalSpecialization specialization;

        private Polyclinic polyclinic;

        private int page;

        private int pageSize;

        private object filter;

        private IPageControlContract pageContract;

        public SpecializationDoctorsViewModel(
            int polyclinicId,
            int specializationId,
            object parentViewModel,
            IMedicalSpecializationDataService specializationDataService,
            IPolyclinicDataService polyclinicDataService,
            IDoctorDataService doctorDataService,
            IProfileDataService profileDataService,
            IEventAggregator eventAggregator)
        {
            this.PolyclinicId = polyclinicId;
            this.SpecializationId = specializationId;
            this.SpecializationDataService = specializationDataService;
            this.PolyclinicDataService = polyclinicDataService;
            this.DoctorDataService = doctorDataService;
            this.ProfileDataService = profileDataService;
            this.EventAggregator = eventAggregator;

            this.PageContract = new PageControlContract(
                this.PolyclinicId,
                this.SpecializationId,
                this.DoctorDataService,
                this.ProfileDataService);

            this.PageSizes = new ObservableCollection<int> { 2, 10, 20, 50, 100, 200 };

            this.LoadedCommand = AsyncCommand.Create(this.OnLoadedAsync);

            this.OpenDoctorCommand = new RelayCommand<int>(
                doctorId => this.EventAggregator.GetEvent<OpenDoctorTimetableEvent>().Publish(
                    new OpenDoctorTimetableEventArgs
                    {
                        DoctorId = doctorId,
                        ParentViewModel = this
                    }));

            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        public ICommand OpenDoctorCommand { get; }

        public ICommand LoadedCommand { get; }

        private async Task OnLoadedAsync()
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

        public IPolyclinicDataService PolyclinicDataService { get; }

        public IDoctorDataService DoctorDataService { get; }

        public IProfileDataService ProfileDataService { get; }

        public IEventAggregator EventAggregator { get; }

        public ObservableCollection<int> PageSizes { get; }

        public IPageControlContract PageContract
        {
            get
            {
                return this.pageContract;
            }

            set
            {
                this.pageContract = value;
                this.OnPropertyChanged();
            }
        }

        public int Page
        {
            get
            {
                return this.page;
            }

            set
            {
                this.page = value;
                this.OnPropertyChanged();
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }

            set
            {
                this.pageSize = value;
                this.OnPropertyChanged();
            }
        }

        public object Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                this.filter = value;
                this.OnPropertyChanged();
            }
        }

        public class PageControlContract : IPageControlContract
        {
            public PageControlContract(int polyclinicId, int specializationId, IDoctorDataService doctorDataService, IProfileDataService profileDataService)
            {
                this.PolyclinicId = polyclinicId;
                this.SpecializationId = specializationId;
                this.DoctorDataService = doctorDataService;
                this.ProfileDataService = profileDataService;
            }

            public int PolyclinicId { get; }

            public int SpecializationId { get; }

            public IDoctorDataService DoctorDataService { get; }

            public IProfileDataService ProfileDataService { get; }

            public async Task<int> GetTotalCountAsync(object filter)
            {
                return await this.DoctorDataService.GetDoctorsCountAsync(
                                                  this.PolyclinicId,
                                                  this.SpecializationId,
                                                  filter as string ?? string.Empty);
            }

            public async Task<IEnumerable> GetRecordsAsync(
                int startingIndex,
                int numberOfRecords,
                object filter)
            {
                IEnumerable<Doctor> doctors = await this.DoctorDataService.GetDoctorsAsync(
                                                  this.PolyclinicId,
                                                  this.SpecializationId,
                                                  startingIndex / numberOfRecords,
                                                  numberOfRecords,
                                                  filter as string ?? string.Empty);

                ICollection<object> doctorsWithProfiles = new List<object>();

                foreach (var doctor in doctors)
                {
                    doctorsWithProfiles.Add(
                        new { Doctor = doctor, Profile = await this.ProfileDataService.GetProfileAsync(doctor.Id) });
                }

                return doctorsWithProfiles;
            }
        }
    }
}
