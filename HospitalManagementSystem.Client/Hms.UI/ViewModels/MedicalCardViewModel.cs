﻿namespace Hms.UI.ViewModels
{
    using System;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.UI.Infrastructure.Commands;

    using MahApps.Metro.Controls.Dialogs;

    public class MedicalCardViewModel : ViewModelBase
    {
        private MedicalCard medicalCard;

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

        public IMedicalCardService MedicalCardService { get; }

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

        public IAsyncCommand LoadedCommand { get; set; }
    }
}
