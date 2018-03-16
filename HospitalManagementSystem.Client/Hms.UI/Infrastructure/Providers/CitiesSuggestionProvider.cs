namespace Hms.UI.Infrastructure.Providers
{
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;

    public class CitiesSuggestionProvider : ICitiesSuggestionProvider
    {
        private YandexGeocoder geocoder = new YandexGeocoder();

        public async Task<IEnumerable> GetSuggestionsAsync(string filter)
        {
            GeoObjectCollection objects = this.geocoder.GeocodeAsync(filter).GetAwaiter().GetResult();

            return objects.Select(geo => geo.GeocoderMetaData).Where(data => data.Kind == GeoObjectKind.Locality)
                .Select(data => this.BuildSuggestion(data.Address)).Where(o => o != null).Distinct().ToList();
        }

        private object BuildSuggestion(Address address)
        {
            if (address.Locality == null || address.Province == null || address.Country == null)
            {
                return null;
            }

            return $"{address.Locality}, {address.Province}, {address.Country}";
        }

        public string SelectedCity { get; set; }
    }
}