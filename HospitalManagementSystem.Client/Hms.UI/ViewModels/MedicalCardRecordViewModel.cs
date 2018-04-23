namespace Hms.UI.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.DataServices.Interface;
    using Hms.UI.Infrastructure.Commands;
    using Hms.UI.Infrastructure.Events;

    using Prism.Events;

    public class MedicalCardRecordViewModel
    {
        private readonly object parentViewModel;

        public MedicalCardRecordViewModel(
            int recordId,
            object parentViewModel,
            IMedicalRecordDataService dataService,
            IEventAggregator eventAggregator)
        {
            this.parentViewModel = parentViewModel;
            this.RecordId = recordId;
            this.DataService = dataService;
            this.EventAggregator = eventAggregator;
            this.LoadedCommand = AsyncCommand.Create(async () => { await Task.CompletedTask; });
            this.BackCommand =
                new RelayCommand(() => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));
        }

        public ICommand LoadedCommand { get; }

        public ICommand BackCommand { get; }

        public int RecordId { get; }

        public IMedicalRecordDataService DataService { get; }

        public IEventAggregator EventAggregator { get; }
    }
}