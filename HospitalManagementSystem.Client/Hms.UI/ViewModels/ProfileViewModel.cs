namespace Hms.UI.ViewModels
{
    using System;
    using System.IO;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.UI.Infrastructure.Commands;

    using MahApps.Metro.Controls.Dialogs;

    using Microsoft.Win32;

    public class ProfileViewModel : ViewModelBase
    {
        private Profile profile;

        public ProfileViewModel(IProfileService profileService, IDialogCoordinator dialogCoordinator)
        {
            this.ProfileService = profileService;
            this.DialogCoordinator = dialogCoordinator;

            this.LoadedCommand = AsyncCommand.Create(
                async () =>
                {
                    try
                    {
                        this.Profile = await this.ProfileService.GetCurrentProfileAsync();
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

                            //TODO
                            //if (img.Width > MAX_IMAGE_WIDTH || img.Height > MAX_IMAGE_HEIGHT)
                            //{
                            //    dialogService.ShowNotification($"Image size should be {MAX_IMAGE_WIDTH} x {MAX_IMAGE_HEIGHT} or less.");
                            //    return;
                            //}

                            Profile.Photo = bytes;

                            await this.ProfileService.InsertOrUpdateProfileAsync(Profile);

                            this.Profile = Profile; // TODO: Implement INPC
                        }
                    }
                    catch (Exception e)
                    {
                        await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                    }
                });
        }

        public IProfileService ProfileService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public IAsyncCommand LoadedCommand { get; set; }

        public IAsyncCommand ChangePhotoCommand { get; set; }

        public Profile Profile
        {
            get
            {
                return this.profile;
            }

            set
            {
                //if (this.profile != value) // TODO
                {
                    this.profile = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
