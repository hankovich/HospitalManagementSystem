namespace Hms.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

        public MedicalCardRecordViewModel(
            int recordId,
            object parentViewModel,
            IMedicalRecordDataService recordDataService,
            IAttachmentDataService attachmentDataService,
            IEventAggregator eventAggregator)
        {
            this.RecordId = recordId;
            this.RecordDataService = recordDataService;
            this.AttachmentDataService = attachmentDataService;
            this.EventAggregator = eventAggregator;
            this.Attachments = new ObservableCollection<Attachment>();

            this.LoadedCommand =
                AsyncCommand.Create(
                    async () =>
                    {
                        var model = await this.RecordDataService.GetMedicalCardRecordAsync(recordId);
                        this.Record = new MedicalCardRecordWrapper(model);

                        if (this.Record.AttachmentIds != null)
                        {
                            foreach (var attachmentId in this.Record.AttachmentIds)
                            {
                                this.Attachments.Add(await this.AttachmentDataService.GetAttachmentAsync(attachmentId));
                            }
                        }
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

        public ObservableCollection<Attachment> Attachments { get; }

        public ICommand LoadedCommand { get; }

        public ICommand BackCommand { get; }

        public int RecordId { get; }

        public IMedicalRecordDataService RecordDataService { get; }

        public IAttachmentDataService AttachmentDataService { get; }

        public IEventAggregator EventAggregator { get; }
    }
}