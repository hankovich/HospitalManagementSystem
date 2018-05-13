namespace Hms.UI.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Hms.Common.Interface.Domain;

    public class CalendarItemWrapper : ModelWrapper<CalendarItem>
    {
        public CalendarItemWrapper(CalendarItem model)
            : base(model)
        {
            this.InitializeComplexProperties(model);
            this.RegisterCollection(this.AssociatedUsers, model.AssociatedUsers);
        }

        private void InitializeComplexProperties(CalendarItem model)
        {
            if (model.AssociatedUsers == null)
            {
                model.AssociatedUsers = new List<User>();
            }

            this.AssociatedUsers =
                new ObservableCollection<UserWrapper>(model.AssociatedUsers.Select(u => new UserWrapper(u)));

            this.Owner = new UserWrapper(model.Owner);
        }

        public int Id
        {
            get
            {
                return this.GetValue<int>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public UserWrapper Owner { get; set; }

        public DateTime StartDate
        {
            get
            {
                return this.GetValue<DateTime>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.GetValue<DateTime>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public string Info
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public ObservableCollection<UserWrapper> AssociatedUsers { get; protected set; }
    }
}
