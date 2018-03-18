namespace Hms.Common.Interface.Geocoding
{
    public struct GeoBound
    {
        public GeoPoint LowerCorner { get; }
        public GeoPoint UpperCorner { get; }

        public GeoBound(GeoPoint lowerCorner, GeoPoint upperCorner)
        {
            this.LowerCorner = lowerCorner;
            this.UpperCorner = upperCorner;
        }

        public override string ToString()
        {
            return $"[{this.LowerCorner}] [{this.UpperCorner}]";
        }

        public override int GetHashCode()
        {
            return this.LowerCorner.GetHashCode() ^ this.UpperCorner.GetHashCode();
        }
    }
}