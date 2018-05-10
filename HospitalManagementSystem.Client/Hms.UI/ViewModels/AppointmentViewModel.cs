namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Geocoding;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Controls.Editors;
    using Hms.UI.Infrastructure.Controls.PagingControl;
    using Hms.UI.Infrastructure.Events;
    using Hms.UI.Wrappers;

    using Prism.Events;

    public class AppointmentViewModel : ViewModelBase
    {
        private AddressWrapper address;

        private int? totalRecords;

        private int page;

        private int pageSize;

        private object filter;

        private IPageControlContract pageContract;

        public AppointmentViewModel(
            IMedicalSpecializationDataService medicalSpecializationDataService,
            IBuildingDataService buildingDataService,
            IProfileDataService profileDataService,
            ISuggestionProvider geoSuggestionProvider,
            IEventAggregator eventAggregator)
        {
            this.MedicalSpecializationDataService = medicalSpecializationDataService;
            this.GeoSuggestionProvider = geoSuggestionProvider;
            this.BuildingDataService = buildingDataService;
            this.ProfileDataService = profileDataService;
            this.EventAggregator = eventAggregator;

            this.PageContract = new PageControlContract(default(int), this.MedicalSpecializationDataService);

            this.PageSizes = new ObservableCollection<int> { 2, 10, 20, 50, 100, 200 };

            this.LoadedCommand = AsyncCommand.Create(this.OnLoaded);
            this.OpenSpecializationCommand = new RelayCommand(
                parameter =>
                {
                    var args = (List<int>)parameter;
                    this.EventAggregator.GetEvent<OpenSpecializationDoctorsEvent>().Publish(
                        new OpenSpecializationDoctorsArgs { PolyclinicId = args[1], SpecializationId = args[0] });
                });
        }

        private async Task OnLoaded()
        {
            await Task.Delay(1000);
            var profile = await this.ProfileDataService.GetCurrentProfileAsync();
            int buildingId = profile.BuildingId.Value;
            var buildingAddress = await this.BuildingDataService.GetBuildingAsync(buildingId);

            var point = new GeoPoint(buildingAddress.Longitude, buildingAddress.Latitude);

            this.Address = new AddressWrapper(
                new Models.Address
                {
                    City = GetCity(buildingAddress, point),
                    Street = GetStreet(buildingAddress, point),
                    Building = GetBuilding(buildingAddress, point)
                });

            this.PageContract = new PageControlContract(buildingAddress.PolyclinicRegion.PolyclinicId, this.MedicalSpecializationDataService);
            this.TotalRecords = await this.PageContract.GetTotalCountAsync(this.Filter);

            this.Address.PropertyChanged += async (sender, args) =>
            {
                if (args.PropertyName == nameof(this.Address.City))
                {
                    this.Address.Street = null;
                    return;
                }

                if (args.PropertyName == nameof(this.Address.Street))
                {
                    this.Address.Building = null;
                    return;
                }

                if (args.PropertyName == nameof(this.Address.Building))
                {
                    if (this.Address.Building == null)
                    {
                        this.TotalRecords = null;
                        this.Filter = null;
                        return;
                    }

                    BuildingAddress ba = await this.BuildingDataService.GetBuildingAsync(this.Address.Building.Point);

                    this.PageContract = new PageControlContract(ba.PolyclinicRegion.PolyclinicId, this.MedicalSpecializationDataService);
                    this.TotalRecords = await this.PageContract.GetTotalCountAsync(this.Filter);
                }
            };
        }

        public AddressWrapper Address
        {
            get
            {
                return this.address;
            }

            set
            {
                this.address = value;
                this.OnPropertyChanged();
            }
        }
        
        public ICommand LoadedCommand { get; }

        public ICommand OpenSpecializationCommand { get; }

        public IBuildingDataService BuildingDataService { get; }

        public IProfileDataService ProfileDataService { get; }

        public IEventAggregator EventAggregator { get; }

        public IMedicalSpecializationDataService MedicalSpecializationDataService { get; }

        public ISuggestionProvider GeoSuggestionProvider { get; }

        public ObservableCollection<int> PageSizes { get; }

        public int? TotalRecords
        {
            get
            {
                return this.totalRecords;
            }

            set
            {
                this.totalRecords = value;
                this.OnPropertyChanged();
            }
        }

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
            public PageControlContract(int polyclinicId, IMedicalSpecializationDataService specializationDataService)
            {
                this.PolyclinicId = polyclinicId;
                this.SpecializationDataService = specializationDataService;
            }

            public int PolyclinicId { get; }

            public IMedicalSpecializationDataService SpecializationDataService { get; }

            public async Task<int> GetTotalCountAsync(object filter)
            {
                if (this.PolyclinicId == default(int))
                {
                    return 0;
                }

                return await this.SpecializationDataService.GetMedicalSpecializationCountAsync(
                           this.PolyclinicId,
                           filter as string ?? string.Empty);
            }

            public async Task<ICollection<object>> GetRecordsAsync(
                int startingIndex,
                int numberOfRecords,
                object filter)
            {
                if (this.PolyclinicId == default(int))
                {
                    return new List<object>();
                }

                IEnumerable<MedicalSpecialization> specializations =
                    await this.SpecializationDataService.GetMedicalSpecializationsAsync(
                        this.PolyclinicId,
                        startingIndex / numberOfRecords,
                        numberOfRecords,
                        filter as string ?? string.Empty);

                return specializations.Cast<object>().ToList();
            }
        }

        private static GeoObject GetBuilding(BuildingAddress buildingAddress, GeoPoint point)
        {
            var address = new Address
            {
                House = buildingAddress.Building
            };

            var geocoderMetaData = new GeoMetaData(buildingAddress.Building, GeoObjectKind.House)
            {
                Address = address
            };

            return new GeoObject
            {
                GeocoderMetaData = geocoderMetaData,
                Point = point
            };
        }

        private static GeoObject GetStreet(BuildingAddress buildingAddress, GeoPoint point)
        {
            var address = GetCityAddress(buildingAddress.City);
            address.Street = buildingAddress.Street;

            var geocoderMetaData = new GeoMetaData(buildingAddress.Street, GeoObjectKind.Street)
            {
                Address = address
            };

            return new GeoObject
            {
                GeocoderMetaData = geocoderMetaData,
                Point = point
            };
        }

        private static GeoObject GetCity(BuildingAddress buildingAddress, GeoPoint point)
        {
            var geocoderMetaData = new GeoMetaData(buildingAddress.City, GeoObjectKind.Locality)
            {
                Address = GetCityAddress(buildingAddress.City)
            };

            return new GeoObject
            {
                GeocoderMetaData = geocoderMetaData,
                Point = point
            };
        }

        private static Address GetCityAddress(string city)
        {
            var tokens = city.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            return new Address
            {
                Locality = tokens[0],
                Province = tokens[1],
                Country = tokens[2]
            };
        }
    }
}
