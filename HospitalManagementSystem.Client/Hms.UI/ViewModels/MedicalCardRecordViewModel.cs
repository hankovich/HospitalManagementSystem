namespace Hms.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
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

        private ProfileWrapper profile;

        public MedicalCardRecordViewModel(
            int recordId,
            object parentViewModel,
            IMedicalRecordDataService recordDataService,
            IAttachmentDataService attachmentDataService,
            IProfileDataService profileDataService,
            IEventAggregator eventAggregator)
        {
            this.RecordId = recordId;
            this.RecordDataService = recordDataService;
            this.AttachmentDataService = attachmentDataService;
            this.ProfileDataService = profileDataService;
            this.EventAggregator = eventAggregator;
            this.Attachments = new ObservableCollection<AttachmentInfoWrapper>();

            this.LoadedCommand = AsyncCommand.Create(this.OnLoadedAsync);
            this.OpenAttachmentCommand = AsyncCommand.Create<int>(this.OnOpenAttachmentAsync);

            this.ShowAttachmentInFolderCommand = AsyncCommand.Create((int attachmentId) =>
            {
                return Task.CompletedTask;
            });

            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));

            this.OpenDoctorCommand = new RelayCommand(() => this.EventAggregator.GetEvent<OpenDoctorEvent>().Publish(new OpenDoctorEventArgs
            {
                Doctor = this.Record.Author,
                ParentViewModel = this
            }));
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

        public ProfileWrapper Profile
        {
            get
            {
                return this.profile;
            }

            set
            {
                this.profile = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<AttachmentInfoWrapper> Attachments { get; }

        public ICommand LoadedCommand { get; }

        public ICommand BackCommand { get; }

        public ICommand OpenAttachmentCommand { get; }

        public ICommand ShowAttachmentInFolderCommand { get; }

        public ICommand OpenDoctorCommand { get; }

        public int RecordId { get; }

        public IMedicalRecordDataService RecordDataService { get; }

        public IAttachmentDataService AttachmentDataService { get; }

        public IProfileDataService ProfileDataService { get; }

        public IEventAggregator EventAggregator { get; }

        private async Task OnLoadedAsync()
        {
            var model = await this.RecordDataService.GetMedicalCardRecordAsync(this.RecordId);
            this.Attachments.Clear();

            var profileModel = await this.ProfileDataService.GetProfileAsync(model.Author.Id);
            this.Profile = new ProfileWrapper(profileModel);

            this.Record = new MedicalCardRecordWrapper(model);

            if (model.AttachmentIds != null)
            {
                foreach (var attachmentId in model.AttachmentIds)
                {
                    var attachment = await this.AttachmentDataService.GetAttachmentInfoAsync(attachmentId);
                    this.Attachments.Add(new AttachmentInfoWrapper(attachment));
                }
            }
        }

        private Task OnOpenAttachmentAsync(int attachmentId)
        {
            var attachment = this.Attachments.FirstOrDefault(att => att.Id == attachmentId);
            if (attachment != null)
            {
                attachment.IsLoading = !attachment.IsLoading;
            }

            return Task.CompletedTask;
        }
    }
}