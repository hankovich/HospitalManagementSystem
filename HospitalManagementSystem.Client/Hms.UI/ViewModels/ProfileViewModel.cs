namespace Hms.UI.ViewModels
{
    using System;
    using System.IO;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.UI.Infrastructure;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Wrappers;

    using MahApps.Metro.Controls.Dialogs;

    public class ProfileViewModel : ViewModelBase
    {
        private ProfileWrapper profile;

        public ProfileViewModel(IProfileDataService profileService, IDialogCoordinator dialogCoordinator, IFileDialogCoordinator fileDialogCoordinator)
        {
            this.FileDialogCoordinator = fileDialogCoordinator;
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
                        string filename = FileDialogCoordinator.OpenFile("Select image file", "Images (*.jpg;*.png)|*.jpg;*.png");

                        if (string.IsNullOrEmpty(filename))
                        {
                            return;
                        }

                        var bytes = File.ReadAllBytes(filename);

                        this.Profile.Photo = bytes;

                        await this.ProfileService.InsertOrUpdateProfileAsync(this.Profile.Model);
                    }
                    catch (Exception e)
                    {
                        await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                    }
                });
        }

        public IProfileDataService ProfileService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        private IFileDialogCoordinator FileDialogCoordinator { get; }
        
        public IAsyncCommand LoadedCommand { get; }

        public IAsyncCommand ChangePhotoCommand { get; }

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
