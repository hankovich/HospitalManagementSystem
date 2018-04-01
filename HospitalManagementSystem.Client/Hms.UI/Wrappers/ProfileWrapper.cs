namespace Hms.UI.Wrappers
{
    using System;
    using System.Collections.Generic;

    using Hms.Common.Interface.Domain;

    public class ProfileWrapper : ModelWrapper<Profile>
    {
        public ProfileWrapper(Profile model)
            : base(model)
        {
        }

        public int Id
        {
            get { return this.GetValue<int>(); }
            set { this.SetValue(value); }
        }

        public int UserId
        {
            get { return this.GetValue<int>(); }
            set { this.SetValue(value); }
        }

        public string FirstName
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        public string MiddleName
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        public string LastName
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        public DateTime DateOfBirth
        {
            get { return this.GetValue<DateTime>(); }
            set { this.SetValue(value); }
        }

        public byte[] Photo
        {
            get { return this.GetValue<byte[]>(); }
            set { this.SetValue(value); }
        }

        public string Phone
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        public int? BuildingId
        {
            get { return this.GetValue<int?>(); }
            set { this.SetValue(value); }
        }

        public int? Entrance
        {
            get { return this.GetValue<int?>(); }
            set { this.SetValue(value); }
        }

        public int? Floor
        {
            get { return this.GetValue<int?>(); }
            set { this.SetValue(value); }
        }

        public int? Flat
        {
            get { return this.GetValue<int?>(); }
            set { this.SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(this.FirstName):
                case nameof(this.MiddleName):
                case nameof(this.LastName):
                case nameof(this.Phone):
                    {
                        if (string.IsNullOrWhiteSpace(this.GetValue<string>(propertyName)))
                        {
                            yield return $"{propertyName} must not be empty";
                        }

                        break;
                    }

                case nameof(this.Entrance):
                    {
                        if (this.Model.Entrance.HasValue && this.Model.Entrance.Value < 1)
                        {
                            yield return "Invalid entrance";
                        }

                        break;
                    }
                case nameof(this.Flat):
                    {
                        if (this.Flat.HasValue && this.Flat.Value <= 0)
                        {
                            yield return "Invalid flat";
                        }

                        break;
                    }
                case nameof(this.Floor):
                    {
                        if (this.Floor.HasValue && this.Floor.Value < 0)
                        {
                            yield return "Invalid floor";
                        }

                        break;
                    }
                case nameof(this.Photo):
                    {
                        if (this.Photo == null)
                        {
                            yield return "Photo is required";
                        }
                        else if (this.Photo.Length == 0)
                        {
                            yield return "Photo must not be blanc";
                        }

                        //TODO
                        //if (img.Width > MAX_IMAGE_WIDTH || img.Height > MAX_IMAGE_HEIGHT)
                        //{
                        //    dialogService.ShowNotification($"Image size should be {MAX_IMAGE_WIDTH} x {MAX_IMAGE_HEIGHT} or less.");
                        //    return;
                        //}

                        break;
                    }
            }
        }
    }
}