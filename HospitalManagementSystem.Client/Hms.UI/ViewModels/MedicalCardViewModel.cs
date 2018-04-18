namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Controls.PagingControl;
    using Hms.UI.Wrappers;

    using MahApps.Metro.Controls.Dialogs;

    using Ninject.Infrastructure.Language;

    public class MedicalCardViewModel : ViewModelBase
    {
        private MedicalCardWrapper medicalCard;

        public MedicalCardViewModel(IMedicalCardDataService service, IDialogCoordinator dialogCoordinator)
        {
            this.MedicalCardService = service;
            this.DialogCoordinator = dialogCoordinator;
            this.PageContract = new PageControlContract(service);

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

        public PageControlContract PageContract { get; }

        public class PageControlContract : IPageControlContract
        {
            public IMedicalCardDataService MedicalCardService { get; }

            public PageControlContract(IMedicalCardDataService medicalCardService)
            {
                this.MedicalCardService = medicalCardService;
            }

            public async Task<int> GetTotalCountAsync()
            {
                return (await this.MedicalCardService.GetMedicalCardAsync(0)).TotalRecords;
            }

            public async Task<ICollection<object>> GetRecordsAsync(int startingIndex, int numberOfRecords, object sortData)
            {
                MedicalCard medicalCard = await this.MedicalCardService.GetMedicalCardAsync(startingIndex / numberOfRecords, numberOfRecords);
                
                return medicalCard.Records.Cast<object>().ToList();
            }
        }
    }
}
