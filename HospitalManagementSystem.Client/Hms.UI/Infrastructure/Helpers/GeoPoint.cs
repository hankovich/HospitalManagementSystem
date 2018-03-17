namespace Hms.UI.Infrastructure.Providers
{
    using System;
    using System.Globalization;

    public struct GeoPoint
    {
        public delegate string ToStringFunc(double x, double y);

        public double Long { get; set; }

        public double Lat { get; set; }

        public static GeoPoint Parse(string point)
        {
            string[] splitted = point.Split(new[] { ' ' }, 2);
            return new GeoPoint(
                double.Parse(splitted[0], CultureInfo.InvariantCulture),
                double.Parse(splitted[1], CultureInfo.InvariantCulture));
        }

        public GeoPoint(double longittude, double latitude)
        {
            this.Long = longittude;
            this.Lat = latitude;
        }


        public override string ToString()
        {
            return $"{this.Long.ToString(CultureInfo.InvariantCulture)} {this.Lat.ToString(CultureInfo.InvariantCulture)}";
        }

        public string ToString(string format)
        {
            return string.Format(
                format,
                this.Long.ToString(CultureInfo.InvariantCulture),
                this.Lat.ToString(CultureInfo.InvariantCulture));
        }

        public string ToString(ToStringFunc formatFunc)
        {
            return formatFunc(this.Long, this.Lat);
        }

        public override int GetHashCode()
        {
            return ((int)this.Long) ^ ((int)this.Lat);
        }
    }
}