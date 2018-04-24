namespace Hms.UI.Wrappers
{
    using System;
    using System.Collections.ObjectModel;

    using Hms.Common.Interface.Domain;

    public class MedicalCardRecordWrapper : ModelWrapper<MedicalCardRecord>
    {
        public MedicalCardRecordWrapper(MedicalCardRecord model)
            : base(model)
        {
        }

        public int Id
        {
            get { return this.GetValue<int>(); }
            set { this.SetValue(value); }
        }

        public int? AssociatedRecordId
        {
            get { return this.GetValue<int?>(); }
            set { this.SetValue(value); }
        }

        public Doctor Author
        {
            get { return this.GetValue<Doctor>(); } // TODO: Create DoctorWrapper
            set { this.SetValue(value); }
        }

        public DateTime AddedAtUtc
        {
            get { return this.GetValue<DateTime>(); }
            set { this.SetValue(value); }
        }

        public DateTime ModifiedAtUtc
        {
            get { return this.GetValue<DateTime>(); }
            set { this.SetValue(value); }
        }

        public string Content
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        public ObservableCollection<int> AttachmentIds
        {
            get { return this.GetValue<ObservableCollection<int>>(); }
            set { this.SetValue(value); }
        }
    }
}