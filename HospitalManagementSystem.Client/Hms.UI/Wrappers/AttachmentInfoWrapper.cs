namespace Hms.UI.Wrappers
{
    using System;

    using Hms.Common.Interface.Domain;

    public class AttachmentInfoWrapper : ModelWrapper<AttachmentInfo>
    {
        private bool isLoading;

        public AttachmentInfoWrapper(AttachmentInfo model)
            : base(model)
        {
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

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                this.OnPropertyChanged();
            }
        }

        public string Name
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

        public int CreatedById
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

        public DateTime CreatedAtUtc
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
    }
}