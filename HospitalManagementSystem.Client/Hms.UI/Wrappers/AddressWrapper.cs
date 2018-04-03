namespace Hms.UI.Wrappers
{
    using System.Collections.Generic;

    using Hms.Common.Interface.Geocoding;

    using Address = Hms.UI.Models.Address;

    public class AddressWrapper : ModelWrapper<Address>
    {
        public AddressWrapper(Address model)
            : base(model)
        {
        }

        public GeoObject City
        {
            get
            {
                return this.GetValue<GeoObject>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        public GeoObject Street
        {
            get
            {
                return this.GetValue<GeoObject>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        public GeoObject Building
        {
            get
            {
                return this.GetValue<GeoObject>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(this.City):
                {
                    if (this.City == null)
                    {
                        yield return $"{propertyName} must not be empty";
                    }

                    break;
                }

                case nameof(this.Street):
                {
                    if (this.Street == null)
                    {
                        yield return $"{propertyName} must not be empty";
                        yield break;
                    }

                    if (this.Street.GeocoderMetaData.Address.Locality != this.City.GeocoderMetaData.Address.Locality)
                    {
                        yield return $"{this.Street} is not in {this.City}";
                    }

                    break;
                }

                case nameof(this.Building):
                {
                    if (this.Building == null)
                    {
                        yield return $"{propertyName} must not be empty";
                        yield break;
                    }

                    if (this.Building.GeocoderMetaData.Address.Street != this.Street.GeocoderMetaData.Address.Street)
                    {
                        yield return $"There is no {this.Building} on {this.Street}";
                    }

                    break;
                }
            }
        }
    }
}