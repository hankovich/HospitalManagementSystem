namespace Hms.Common.Interface.Geocoding
{
    using System.Threading.Tasks;

    public interface IGeocoder
    {
        Task<GeoObjectCollection> GeocodeAsync(string location);

        Task<GeoObjectCollection> GeocodeAsync(string location, short results);

        Task<GeoObjectCollection> GeocodeAsync(string location, short results, LangType lang);

        Task<GeoObjectCollection> GeocodeAsync(
            string location,
            short results,
            LangType lang,
            SearchArea searchArea,
            bool rspn = false);

        Task<GeoObjectCollection> GeocodeAsync(
            string location,
            short results,
            LangType lang,
            GeoBound geoBound,
            bool rspn = false);

        Task<GeoObjectCollection> ReverseGeocodeAsync(GeoPoint point);

        Task<GeoObjectCollection> ReverseGeocodeAsync(GeoPoint point, GeoObjectKind kind);

        Task<GeoObjectCollection> ReverseGeocodeAsync(GeoPoint point, GeoObjectKind kind, short results);

        Task<GeoObjectCollection> ReverseGeocodeAsync(GeoPoint point, GeoObjectKind kind, short results, LangType lang);
    }
}