namespace Hms.UI.ViewModels
{
    using System;

    using Hms.Services.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Wrappers;

    using MahApps.Metro.Controls.Dialogs;

    public class MedicalCardViewModel : ViewModelBase
    {
        private MedicalCardWrapper medicalCard;

        public MedicalCardViewModel(IMedicalCardDataService service, IDialogCoordinator dialogCoordinator)
        {
            this.MedicalCardService = service;
            this.DialogCoordinator = dialogCoordinator;

            this.LoadedCommand = AsyncCommand.Create(async () =>
            {
                try
                {
                    var card = await this.MedicalCardService.GetMedicalCardAsync(0);
                    this.MedicalCard = new MedicalCardWrapper(card);
                }
                catch (Exception e)
                {
                    await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                }
            });
        }

        public IMedicalCardDataService MedicalCardService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public MedicalCardWrapper MedicalCard
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

        public IAsyncCommand LoadedCommand { get; }
    }
}
