namespace Hms.UI.Infrastructure.Providers
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;

    public class StreetsSuggestionProvider : IStreetsSuggestionProvider
    {
        private YandexGeocoder geocoder = new YandexGeocoder();

        public StreetsSuggestionProvider(ICitiesSuggestionProvider citiesSuggestionProvider)
        {
            this.CitiesSuggestionProvider = citiesSuggestionProvider;
        }

        public async Task<IEnumerable> GetSuggestionsAsync(string filter)
        {
            GeoObjectCollection objects = this.geocoder.GeocodeAsync(filter, 10000, LangType.RU, new SearchArea()).GetAwaiter().GetResult();

            return objects.Select(geo => geo.GeocoderMetaData).Where(data => data.Kind == GeoObjectKind.Street)
                .Select(data => this.BuildSuggestion(data.Address)).Where(o => o != null).Distinct().ToList();
        }

        private object BuildSuggestion(Address address)
        {
            if (address.Street == null)
            {
                return null;
            }

            var tokens = this.CitiesSuggestionProvider.SelectedCity.Split(
                new[] { ',', ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            if (address.Locality == tokens[0] && address.Province == tokens[1] && address.Country == tokens[2])
            {
                return address.Street;
            }
            
            return null;
        }

        public ICitiesSuggestionProvider CitiesSuggestionProvider { get; }

        public string SelectedStreet { get; set; }
    }
}