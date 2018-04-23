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

        private int page;

        private int pageSize;

        private object filter;

        private int totalPages;

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

        public int Page
        {
            get
            {
                return this.page;
            }

            set
            {
                this.page = value;
                this.OnPropertyChanged();
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }

            set
            {
                this.pageSize = value;
                this.OnPropertyChanged();
            }
        }

        public int TotalPages
        {
            get
            {
                return this.totalPages;
            }

            set
            {
                this.totalPages = value;
                this.OnPropertyChanged();
            }
        }

        public object Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                this.filter = value;
                this.OnPropertyChanged();
            }
        }

        public class PageControlContract : IPageControlContract
        {
            public PageControlContract(IMedicalCardDataService medicalCardService)
            {
                this.MedicalCardService = medicalCardService;
            }

            public IMedicalCardDataService MedicalCardService { get; }

            public async Task<int> GetTotalCountAsync(object filter)
            {
                return (await this.MedicalCardService.GetMedicalCardAsync(0, 20, filter as string ?? string.Empty)).TotalRecords;
            }

            public async Task<ICollection<object>> GetRecordsAsync(int startingIndex, int numberOfRecords, object filter)
            {
                MedicalCard medicalCard = await this.MedicalCardService.GetMedicalCardAsync(startingIndex / numberOfRecords, numberOfRecords, filter as string ?? string.Empty);
                
                return medicalCard.Records.Cast<object>().ToList();
            }
        }
    }
}
