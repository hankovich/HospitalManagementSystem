namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Controls.PagingControl;

    using MahApps.Metro.Controls.Dialogs;

    public class MedicalCardViewModel : ViewModelBase
    {
        private int? totalRecords;

        public MedicalCardViewModel(IMedicalCardDataService service, IDialogCoordinator dialogCoordinator)
        {
            this.MedicalCardService = service;
            this.DialogCoordinator = dialogCoordinator;
            this.PageContract = new PageControlContract(service);

            this.PageSizes = new ObservableCollection<int> { 2, 10, 20, 50, 100, 200 };

            this.LoadedCommand = AsyncCommand.Create(async () =>
            {
                try
                {
                    var card = await this.MedicalCardService.GetMedicalCardAsync(0);
                    this.TotalRecords = card.TotalRecords;
                }
                catch (Exception e)
                {
                    await this.DialogCoordinator.ShowMessageAsync(this, "Oops", e.Message);
                }
            });
        }

        public IMedicalCardDataService MedicalCardService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public IAsyncCommand LoadedCommand { get; }

        public ObservableCollection<int> PageSizes { get; }

        public int? TotalRecords
        {
            get
            {
                return this.totalRecords;
            }

            set
            {
                this.totalRecords = value;
                this.OnPropertyChanged();
            }
        }

        public PageControlContract PageContract { get; }

        public class PageControlContract : IPageControlContract
        {
            public PageControlContract(IMedicalCardDataService medicalCardService)
            {
                this.MedicalCardService = medicalCardService;
            }

            public IMedicalCardDataService MedicalCardService { get; }

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
