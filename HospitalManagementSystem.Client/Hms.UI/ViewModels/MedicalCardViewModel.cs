namespace Hms.UI.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.UI.Annotations;
    using Hms.UI.Infrastructure.Commands;

    using MahApps.Metro.Controls.Dialogs;

    public class MedicalCardViewModel : INotifyPropertyChanged
    {
        public MedicalCardViewModel(IMedicalCardService service, IDialogCoordinator dialogCoordinator)
        {
            this.MedicalCardService = service;
            this.DialogCoordinator = dialogCoordinator;

            this.LoadedCommand = AsyncCommand.Create(async () =>
            {
                try
                {
                    this.MedicalCard = await this.MedicalCardService.GetMedicalCardAsync(0);
                }
                catch (Exception e)
                {
                    await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                }
            });
        }

        public IMedicalCardService MedicalCardService { get; set; }

        public IDialogCoordinator DialogCoordinator { get; }

        public MedicalCard MedicalCard
        {
            get
            {
                return this.medicalCard;
            }

            set
            {
                if (medicalCard != value)
                {
                    this.medicalCard = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private MedicalCard medicalCard;

        public IAsyncCommand LoadedCommand { get; set; }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
