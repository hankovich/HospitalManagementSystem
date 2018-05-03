namespace Hms.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Hms.Common.Interface.Domain;
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

            this.LoadedCommand =
                AsyncCommand.Create(
                    async () =>
                    {
                        var model = await this.RecordDataService.GetMedicalCardRecordAsync(recordId);

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
                    });

            this.BackCommand = new RelayCommand(
                () => this.EventAggregator.GetEvent<NavigationEvent>().Publish(parentViewModel));

            this.OpenAttachmentCommand = AsyncCommand.Create(
                (int attachmentId) =>
                {
                    var attachment = this.Attachments.FirstOrDefault(att => att.Id == attachmentId);
                    if (attachment != null)
                    {
                        attachment.IsLoading = !attachment.IsLoading;
                    }

                    return Task.CompletedTask;
                });

            this.ShowAttachmentInFolderCommand = AsyncCommand.Create((int attachmentId) =>
            {
                return Task.CompletedTask;
            });
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

        public int RecordId { get; }

        public IMedicalRecordDataService RecordDataService { get; }

        public IAttachmentDataService AttachmentDataService { get; }

        public IProfileDataService ProfileDataService { get; set; }

        public IEventAggregator EventAggregator { get; }
    }
}