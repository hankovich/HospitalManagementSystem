namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Geocoding;
    using Hms.Services.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Controls.Editors;
    using Hms.UI.Wrappers;

    using MahApps.Metro.Controls.Dialogs;

    using Microsoft.Win32;

    public class CreateProfileViewModel : NotifyDataErrorInfoBase
    {
        #region Backing fields

        private GeoObject city;

        private GeoObject street;

        private GeoObject building;

        private ProfileWrapper profile;

        #endregion

        public CreateProfileViewModel(ISuggestionProvider geoSuggestionProvider, IProfileDataService profileService, IBuildingDataService buildingService, IDialogCoordinator dialogCoordinator)
        {
            this.GeoSuggestionProvider = geoSuggestionProvider;
            this.ProfileService = profileService;
            this.BuildingService = buildingService;
            this.DialogCoordinator = dialogCoordinator;

            this.Profile = new ProfileWrapper(new Profile
            {
                DateOfBirth = DateTime.Now
            });

            this.AddPhotoCommand = new RelayCommand(this.OpenPhotoAsBytes);
            this.ValidateCommand = AsyncCommand.Create(this.ValidateAsync);
            this.CreateProfileCommand = AsyncCommand.Create(this.CreateAndSaveProfileAsync);
        }

        public ISuggestionProvider GeoSuggestionProvider { get; }

        public IProfileDataService ProfileService { get; }

        public IBuildingDataService BuildingService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public ICommand AddPhotoCommand { get; }

        public ICommand ValidateCommand { get; }

        public ICommand CreateProfileCommand { get; }

        public ProfileWrapper Profile
        {
            get
            {
                return this.profile;
            }

            set
            {
                if (this.profile != value)
                {
                    this.profile = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public GeoObject City
        {
            get
            {
                return this.city;
            }

            set
            {
                this.city = value;
                this.OnPropertyChanged();

                if (value == null)
                {
                    this.AddError();
                    this.OnErrorsChanged();
                }
                else
                {
                    this.ClearErrors();
                }
            }
        }

        public GeoObject Street
        {
            get
            {
                return this.street;
            }

            set
            {
                this.street = value;
                this.OnPropertyChanged();

                if (value == null)
                {
                    this.AddError();
                    this.OnErrorsChanged();
                }
                else
                {
                    this.ClearErrors();
                }
            }
        }

        public GeoObject Building
        {
            get
            {
                return this.building;
            }

            set
            {
                this.building = value;
                this.OnPropertyChanged();

                if (value == null)
                {
                    this.AddError();
                    this.OnErrorsChanged();
                }
                else
                {
                    this.ClearErrors();
                }
            }
        }

        private async Task CreateAndSaveProfileAsync()
        {
            try
            {
                int userId = this.ProfileService.Client.UserId.Value;

                BuildingAddress address = await this.BuildingService.GetBuildingAsync(this.Building.Point);

                Profile model = this.Profile.Model;

                model.UserId = userId;
                model.BuildingId = address.Id;

                await this.ProfileService.InsertOrUpdateProfileAsync(model);
            }
            catch (Exception e)
            {
                await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                throw;
            }
        }

        private async Task ValidateAsync()
        {
            try
            {
                this.Profile.Revalidate();
                this.UpdateAddressErrors();

                if (this.Profile.HasErrors || this.HasErrors)
                {
                    throw new ArgumentException("There are some validation errors!");
                }
            }
            catch (Exception e)
            {
                await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                throw;
            }
        }

        private void UpdateAddressErrors()
        {
            IEnumerable<PropertyInfo> geoProperties = typeof(CreateProfileViewModel).GetProperties().Where(pi => pi.PropertyType == typeof(GeoObject));

            foreach (var geoProperty in geoProperties)
            {
                ValidateGeoObject((GeoObject)geoProperty.GetValue(this), geoProperty.Name);
            }
        }

        private void ValidateGeoObject(GeoObject propertyValue, string propertyName)
        {
            if (propertyValue == null)
            {
                this.AddError(string.Empty, propertyName);
                this.OnErrorsChanged(propertyName);
            }
        }

        private void OpenPhotoAsBytes()
        {
            OpenFileDialog diag = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Title = "Select image file",
                Filter = "Images (*.jpg;*.png)|*.jpg;*.png",
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true
            };

            if (diag.ShowDialog() == true)
            {
                var bytes = File.ReadAllBytes(diag.FileName);
                this.Profile.Photo = bytes;
            }
        }
    }
}