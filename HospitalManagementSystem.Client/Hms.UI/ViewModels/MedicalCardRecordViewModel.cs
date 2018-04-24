namespace Hms.UI.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;
    using Hms.UI.Wrappers;

    using Prism.Events;

    public class MedicalCardRecordViewModel : ViewModelBase
    {
        private MedicalCardRecordWrapper record;

        public MedicalCardRecordViewModel(
            int recordId,
            object parentViewModel,
            IMedicalRecordDataService dataService,
            IEventAggregator eventAggregator)
        {
            this.RecordId = recordId;
            this.DataService = dataService;
            this.EventAggregator = eventAggregator;
            this.LoadedCommand =
                AsyncCommand.Create(
                    async () =>
                    {
                        var model = await this.DataService.GetMedicalCardRecordAsync(recordId);
                        this.Record = new MedicalCardRecordWrapper(model);
                    });
            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        public MedicalCardRecordWrapper Record
        {
            get
            {
                return this.record;
            }

            set
            {
                this.record = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand LoadedCommand { get; }

        public ICommand BackCommand { get; }

        public int RecordId { get; }

        public IMedicalRecordDataService DataService { get; }

        public IEventAggregator EventAggregator { get; }
    }
}