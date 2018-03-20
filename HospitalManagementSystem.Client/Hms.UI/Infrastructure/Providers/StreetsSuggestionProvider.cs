namespace Hms.UI.Infrastructure.Providers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Hms.Common.Interface.Geocoding;

    public class StreetsSuggestionProvider : IStreetsSuggestionProvider
    {
        private readonly IGeoSuggester geoSuggester;

        private readonly IGeocoder geocoder;

        public StreetsSuggestionProvider(ICitiesSuggestionProvider citiesSuggestionProvider)
        {
            this.CitiesSuggestionProvider = citiesSuggestionProvider;

            this.geocoder = this.CitiesSuggestionProvider.Geocoder;
            this.geoSuggester = this.CitiesSuggestionProvider.GeoSuggester;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            if (this.CitiesSuggestionProvider.SelectedCity == null)
            {
                return Enumerable.Empty<object>();
            }

            GeoObject city = this.CitiesSuggestionProvider.SelectedCity;

            IEnumerable<string> suggestions = this.geoSuggester.SuggestAsync(this.BuildFilter(city, filter)).GetAwaiter().GetResult().Take(100);

            GeoObjectCollection objects = new GeoObjectCollection(suggestions.AsParallel().SelectMany(elem => this.geocoder.GeocodeAsync(elem, 5).GetAwaiter().GetResult()));

            return objects.Where(o => this.IsStreetInCity(o, city)).Distinct().ToList();
        }

        private bool IsStreetInCity(GeoObject data, GeoObject city)
        {
            if (data.GeocoderMetaData.Kind == GeoObjectKind.Street && !string.IsNullOrEmpty(data.ToString())
                && data.GeocoderMetaData.Address.Locality == city.GeocoderMetaData.Address.Locality
                && data.GeocoderMetaData.Address.Province == city.GeocoderMetaData.Address.Province)
            {
                return true;
            }

            return false;
        }

        private string BuildFilter(GeoObject city, string filter)
        {
            return $"{city.GeocoderMetaData.Address.Locality} {filter}";
        }

        public ICitiesSuggestionProvider CitiesSuggestionProvider { get; }

        public GeoObject SelectedStreet { get; set; }
    }
}