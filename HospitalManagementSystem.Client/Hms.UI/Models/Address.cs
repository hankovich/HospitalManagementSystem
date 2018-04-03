namespace Hms.UI.Models
{
    using Hms.Common.Interface.Geocoding;

    public class Address
    {
        public GeoObject City { get; set; }
        public GeoObject Street { get; set; }
        public GeoObject Building { get; set; }
    }
}
