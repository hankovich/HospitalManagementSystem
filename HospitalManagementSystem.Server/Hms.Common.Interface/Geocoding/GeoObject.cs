namespace Hms.Common.Interface.Geocoding
{
    using System;

    public class GeoObject : IEquatable<GeoObject>
    {
        public GeoPoint Point { get; set; }

        public GeoBound BoundedBy { get; set; }

        public GeoMetaData GeocoderMetaData { get; set; }

        public bool Equals(GeoObject other)
        {
            return this.ToString() == other?.ToString();
        }

        public override string ToString()
        {
            Address address = this.GeocoderMetaData.Address;

            if (!string.IsNullOrEmpty(address.House))
            {
                return address.House;
            }

            if (this.GeocoderMetaData.Kind == GeoObjectKind.Locality)
            {
                if (address.Locality == null || address.Province == null || address.Country == null)
                {
                    return string.Empty;
                }

                return $"{address.Locality}, {address.Province}, {address.Country}";
            }

            if (this.GeocoderMetaData.Kind == GeoObjectKind.Street || this.GeocoderMetaData.Kind == GeoObjectKind.House)
            {
                return $"{address.Street}";
            }

            throw new Exception();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}