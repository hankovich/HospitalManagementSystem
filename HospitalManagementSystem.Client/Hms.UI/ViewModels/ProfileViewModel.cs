namespace Hms.UI.ViewModels
{
    using System;
    using System.IO;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Wrappers;

    using MahApps.Metro.Controls.Dialogs;

    using Microsoft.Win32;

    public class ProfileViewModel : ViewModelBase
    {
        private ProfileWrapper profile;

        public ProfileViewModel(IProfileDataService profileService, IDialogCoordinator dialogCoordinator)
        {
            this.ProfileService = profileService;
            this.DialogCoordinator = dialogCoordinator;

            this.LoadedCommand = AsyncCommand.Create(
                async () =>
                {
                    try
                    {
                        Profile model = await this.ProfileService.GetCurrentProfileAsync();
                        this.Profile = new ProfileWrapper(model);
                    }
                    catch (Exception e)
                    {
                        await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                    }
                });

            this.ChangePhotoCommand = AsyncCommand.Create(
                async () =>
                {
                    try
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

                            await this.ProfileService.InsertOrUpdateProfileAsync(this.Profile.Model);
                        }
                    }
                    catch (Exception e)
                    {
                        await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                    }
                });
        }

        public IProfileDataService ProfileService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public IAsyncCommand LoadedCommand { get; set; }

        public IAsyncCommand ChangePhotoCommand { get; set; }

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
    }
}
