namespace Hms.Common.Interface.Geocoding
{
    using System.Globalization;

    public struct GeoPoint
    {
        public double Longittude { get; }

        public double Latitude { get; }

        public static GeoPoint Parse(string point)
        {
            string[] splitted = point.Split(new[] { ' ' }, 2);
            return new GeoPoint(
                double.Parse(splitted[0], CultureInfo.InvariantCulture),
                double.Parse(splitted[1], CultureInfo.InvariantCulture));
        }

        public GeoPoint(double longitude, double latitude)
        {
            this.Longittude = longitude;
            this.Latitude = latitude;
        }


        public override string ToString()
        {
            return $"{this.Longittude.ToString(CultureInfo.InvariantCulture)} {this.Latitude.ToString(CultureInfo.InvariantCulture)}";
        }

        public string ToString(string format)
        {
            return string.Format(
                format,
                this.Longittude.ToString(CultureInfo.InvariantCulture),
                this.Latitude.ToString(CultureInfo.InvariantCulture));
        }

        public override int GetHashCode()
        {
            return ((int)this.Longittude) ^ (int)this.Latitude;
        }
    }
}