namespace Hms.UI.Infrastructure.Providers
{
    public struct GeoBound
    {
        public GeoPoint lowerCorner, upperCorner;

        public GeoBound(GeoPoint lowerCorner, GeoPoint upperCorner)
        {
            this.lowerCorner = lowerCorner;
            this.upperCorner = upperCorner;
        }

        public override string ToString()
        {
            return $"[{this.lowerCorner}] [{this.upperCorner}]";
        }
    }
}