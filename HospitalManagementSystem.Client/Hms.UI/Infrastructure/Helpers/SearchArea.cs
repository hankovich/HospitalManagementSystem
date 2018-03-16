namespace Hms.UI.Infrastructure.Providers
{
    public struct SearchArea
    {
        public GeoPoint longLat, spread;

        public SearchArea(GeoPoint CenterLongLat, GeoPoint Spread)
        {
            this.longLat = CenterLongLat;
            this.spread = Spread;
        }
    }
}