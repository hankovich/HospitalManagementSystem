namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Geocoding;
    using Hms.Services.Interface;
    using Hms.UI.Annotations;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Providers;

    using MahApps.Metro.Controls.Dialogs;

    using Microsoft.Win32;

    public class CreateProfileViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public IBuildingsSuggestionProvider BuildingsSuggestionProvider { get; }

        public IProfileService ProfileService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public ICommand AddPhotoCommand { get; set; }

        public ICommand ValidateCommand { get; set; }

        public ICommand CreateProfileCommand { get; set; }

        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        #region Backing fields

        private GeoObject city;

        private GeoObject street;

        private GeoObject building;

        private string firstName;

        private string middleName;

        private string lastName;

        private DateTime? dateOfBirth;

        private int entrance;

        private int floor;

        private int flat;

        private string phone;

        private byte[] photo;

        #endregion

        public CreateProfileViewModel(IBuildingsSuggestionProvider buildingsProvider, IProfileService profileService, IDialogCoordinator dialogCoordinator)
        {
            this.BuildingsSuggestionProvider = buildingsProvider;
            this.ProfileService = profileService;
            this.DialogCoordinator = dialogCoordinator;

            this.AddPhotoCommand = new RelayCommand(this.OpenPhotoAsBytes);
            this.ValidateCommand = AsyncCommand.Create(this.ValidateAsync);
            this.CreateProfileCommand = AsyncCommand.Create(this.CreateAndSaveProfileAsync);
        }

        private async Task CreateAndSaveProfileAsync()
        {
            try
            {
                int userId = this.ProfileService.Client.UserId.Value;

                //int buildingId = await this.BuildingService. ;
                Profile profile = new Profile
                {
                    UserId = userId,
                    DateOfBirth = this.DateOfBirth.Value,
                    FirstName = this.FirstName,
                    MiddleName = this.MiddleName,
                    LastName = this.LastName,
                    Entrance = this.Entrance,
                    Flat = this.Flat,
                    Floor = this.Floor,
                    Phone = this.Phone,
                    Photo = this.Photo
                };

                await this.ProfileService.InsertOrUpdateProfileAsync(profile);
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
                if (string.IsNullOrWhiteSpace(this.FirstName) || string.IsNullOrWhiteSpace(this.MiddleName)
                    || string.IsNullOrWhiteSpace(this.LastName) || this.Building == null || this.Entrance < 1
                    || this.Floor < 0 || this.Flat <= 0 || this.Photo == null || this.Photo.Length == 0
                    || string.IsNullOrWhiteSpace(this.Phone))
                {
                    throw new ArgumentException("All fields are required!");
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

                //if (img.Width > MAX_IMAGE_WIDTH || img.Height > MAX_IMAGE_HEIGHT)
                //{
                //    dialogService.ShowNotification($"Image size should be {MAX_IMAGE_WIDTH} x {MAX_IMAGE_HEIGHT} or less.");
                //    return;
                //}

                this.Photo = bytes;
            }
        }

        #region Properties

        public GeoObject City
        {
            get
            {
                return this.city;
            }

            set
            {
                string key = nameof(this.City);
                this.city = value;

                this.UpdateErrors(key, value != null);
                this.BuildingsSuggestionProvider.StreetsSuggestionProvider.CitiesSuggestionProvider.SelectedCity = value;

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
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
                string key = nameof(this.Street);
                this.street = value;

                this.UpdateErrors(key, value != null);
                this.BuildingsSuggestionProvider.StreetsSuggestionProvider.SelectedStreet = value;

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
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
                string key = nameof(this.Building);
                this.building = value;

                this.UpdateErrors(key, value != null);
                this.BuildingsSuggestionProvider.SelectedBuilding = value;

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
            }
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                string key = nameof(this.FirstName);
                this.firstName = value;

                this.UpdateErrors(key, !string.IsNullOrWhiteSpace(value));

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
            }
        }

        public string MiddleName
        {
            get
            {
                return this.middleName;
            }

            set
            {
                string key = nameof(this.MiddleName);
                this.middleName = value;

                this.UpdateErrors(key, !string.IsNullOrWhiteSpace(value));

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                string key = nameof(this.LastName);
                this.lastName = value;

                this.UpdateErrors(key, !string.IsNullOrWhiteSpace(value));

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
            }
        }

        public DateTime? DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }

            set
            {
                string key = nameof(this.DateOfBirth);
                this.dateOfBirth = value;

                this.UpdateErrors(key, value != null);

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
            }
        }

        public int Entrance
        {
            get
            {
                return this.entrance;
            }

            set
            {
                this.entrance = value;
                this.OnPropertyChanged();
            }
        }

        public int Floor
        {
            get
            {
                return this.floor;
            }

            set
            {
                this.floor = value;
                this.OnPropertyChanged();
            }
        }

        public int Flat
        {
            get
            {
                return this.flat;
            }

            set
            {
                this.flat = value;
                this.OnPropertyChanged();
            }
        }

        public string Phone
        {
            get
            {
                return this.phone;
            }

            set
            {
                string key = nameof(this.Phone);
                this.phone = value;

                this.UpdateErrors(key, !string.IsNullOrWhiteSpace(value));

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
            }
        }

        public byte[] Photo
        {
            get
            {
                return this.photo;
            }

            set
            {
                string key = nameof(this.Phone);
                this.photo = value;

                this.UpdateErrors(key, value != null && value.Length > 0);

                this.OnPropertyChanged();
                this.OnRaiseErrorsChanged();
            }
        }

        #endregion

        #region INotify... members

        public IEnumerable GetErrors(string propertyName)
        {
            return this.errors.FirstOrDefault(error => error.Key == propertyName).Value;
        }

        private void UpdateErrors(string key, bool isValidNow, string error = "Field must not be empty")
        {
            if (isValidNow)
            {
                List<string> propertyErrors;

                if (this.errors.TryGetValue(key, out propertyErrors))
                {
                    propertyErrors.Clear();
                }
            }
            else
            {
                if (!this.errors.ContainsKey(key))
                {
                    this.errors.Add(key, new List<string>());
                }

                this.errors[key].Add(error);
            }
        }

        public bool HasErrors => this.errors.Values.FirstOrDefault(l => l.Count > 0) != null;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnRaiseErrorsChanged([CallerMemberName] string propertyName = null)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion
    }
}