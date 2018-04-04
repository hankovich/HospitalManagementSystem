namespace Hms.UI.ViewModels
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Controls.Editors;
    using Hms.UI.Models;
    using Hms.UI.Wrappers;

    using MahApps.Metro.Controls.Dialogs;

    public class CreateProfileViewModel : NotifyDataErrorInfoBase
    {
        private ProfileWrapper profile;
        private AddressWrapper address;
        
        public CreateProfileViewModel(ISuggestionProvider geoSuggestionProvider, IProfileDataService profileService, IBuildingDataService buildingService, IDialogCoordinator dialogCoordinator, IFileDialogCoordinator fileDialogCoordinator)
        {
            this.GeoSuggestionProvider = geoSuggestionProvider;
            this.ProfileService = profileService;
            this.BuildingService = buildingService;
            this.DialogCoordinator = dialogCoordinator;
            this.FileDialogCoordinator = fileDialogCoordinator;

            this.Profile = new ProfileWrapper(new Profile
            {
                DateOfBirth = DateTime.Now
            });

            this.Address = new AddressWrapper(new Address());

            this.AddPhotoCommand = new RelayCommand(this.OpenPhotoAsBytes);
            this.ValidateCommand = AsyncCommand.Create(this.ValidateAsync);
            this.CreateProfileCommand = AsyncCommand.Create(this.CreateAndSaveProfileAsync);
        }

        public ISuggestionProvider GeoSuggestionProvider { get; }

        public IProfileDataService ProfileService { get; }

        public IBuildingDataService BuildingService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public IFileDialogCoordinator FileDialogCoordinator { get; }

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

        public AddressWrapper Address
        {
            get
            {
                return this.address;
            }

            set
            {
                if (this.address != value)
                {
                    this.address = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private async Task CreateAndSaveProfileAsync()
        {
            try
            {
                int userId = this.ProfileService.Client.UserId.Value;

                BuildingAddress buildingAddress = await this.BuildingService.GetBuildingAsync(this.Address.Building.Point);

                Profile model = this.Profile.Model;

                model.UserId = userId;
                model.BuildingId = buildingAddress.Id;

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
                this.Address.Revalidate();

                if (this.Profile.HasErrors || this.Address.HasErrors)
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

        private void OpenPhotoAsBytes()
        {
            string filename = this.FileDialogCoordinator.OpenFile("Select image file", "Images (*.jpg;*.png)|*.jpg;*.png");

            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            var bytes = File.ReadAllBytes(filename);
            this.Profile.Photo = bytes;
        }
    }
}