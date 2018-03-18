namespace Hms.Common.Interface.Geocoding
{
    public class GeoMetaData
    {
        public GeoObjectKind Kind { get; set; } = GeoObjectKind.Locality;

        public string Text { get; set; } = string.Empty;

        public Address Address { get; set; }

        public GeoMetaData()
        {
        }

        public GeoMetaData(string text)
        {
            this.Text = text;
        }

        public GeoMetaData(string text, string kind) : this(text)
        {
            this.Kind = ParseKind(kind);
        }

        public GeoMetaData(string text, GeoObjectKind kind) : this(text)
        {
            this.Kind = kind;
        }

        public GeoMetaData(string text, string kind, Address address) : this(text, kind)
        {
            this.Address = address;
        }

        public GeoMetaData(string text, GeoObjectKind kind, Address address) : this(text, kind)
        {
            this.Address = address;
        }

        public static GeoObjectKind ParseKind(string kind)
        {
            switch (kind.ToLower())
            {
                case "district": return GeoObjectKind.District;
                case "house": return GeoObjectKind.House;
                case "locality": return GeoObjectKind.Locality;
                case "country": return GeoObjectKind.Country;
                case "province": return GeoObjectKind.Province;
                case "metro": return GeoObjectKind.Metro;
                case "street": return GeoObjectKind.Street;
                default: return GeoObjectKind.Locality;
            }
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}