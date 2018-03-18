namespace Hms.Common.Interface.Geocoding
{
    public struct SearchArea
    {
        public GeoPoint Center { get; }

        public GeoPoint Spread { get; }

        public SearchArea(GeoPoint centerLongLat, GeoPoint spread)
        {
            this.Center = centerLongLat;
            this.Spread = spread;
        }
    }
}