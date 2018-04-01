namespace Hms.UI.Wrappers
{
    using System;
    using System.Collections.Generic;

    using Hms.Common.Interface.Domain;

    public class MedicalCardWrapper : ModelWrapper<MedicalCard>
    {
        public MedicalCardWrapper(MedicalCard model)
            : base(model)
        {
        }

        public int Id
        {
            get { return this.GetValue<int>(); }
            set { this.SetValue(value); }
        }

        public User User
        {
            get { return this.GetValue<User>(); }
            set { this.SetValue(value); }
        }

        public DateTime StartedAtUtc
        {
            get { return this.GetValue<DateTime>(); }
            set { this.SetValue(value); }
        }

        public int TotalRecords
        {
            get { return this.GetValue<int>(); }
            set { this.SetValue(value); }
        }

        public ICollection<MedicalCardRecord> Records
        {
            get { return this.GetValue<ICollection<MedicalCardRecord>>(); }
            set { this.SetValue(value); }
        }
    }
}