namespace Hms.UI.Infrastructure.Providers
{
    public class GeoObject
    {
        public GeoPoint Point { get; set; }

        public GeoBound BoundedBy { get; set; }

        public GeoMetaData GeocoderMetaData { get; set; }
    }
}