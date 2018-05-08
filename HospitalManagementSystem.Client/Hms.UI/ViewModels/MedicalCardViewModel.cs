namespace Hms.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Controls.PagingControl;
    using Hms.UI.Infrastructure.Events;

    using MahApps.Metro.Controls.Dialogs;

    using Prism.Events;

    public class MedicalCardViewModel : ViewModelBase
    {
        private int? totalRecords;

        private int page;

        private int pageSize;

        private object filter;

        public MedicalCardViewModel(
            IMedicalCardDataService service,
            IDialogCoordinator dialogCoordinator,
            IEventAggregator eventAggregator)
        {
            this.MedicalCardService = service;
            this.DialogCoordinator = dialogCoordinator;
            this.EventAggregator = eventAggregator;
            this.PageContract = new PageControlContract(service);

            this.PageSizes = new ObservableCollection<int> { 2, 10, 20, 50, 100, 200 };

            this.LoadedCommand = AsyncCommand.Create(
                async () =>
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

            this.OpenRecordCommand = new RelayCommand<int>(
                recordId =>
                {
                    var @event = this.EventAggregator.GetEvent<OpenRecordEvent>();
                    @event.Publish(new OpenRecordEventArgs { RecordId = recordId, ParentViewModel = this });
                });
        }

        public IMedicalCardDataService MedicalCardService { get; }

        public IDialogCoordinator DialogCoordinator { get; }

        public IEventAggregator EventAggregator { get; }

        public ICommand LoadedCommand { get; }

        public ICommand OpenRecordCommand { get; }

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

        public IPageControlContract PageContract { get; }

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
