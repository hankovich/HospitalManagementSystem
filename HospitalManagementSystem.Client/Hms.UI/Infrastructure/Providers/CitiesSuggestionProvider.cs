namespace Hms.UI.Infrastructure.Providers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Hms.UI.Infrastructure.Helpers;

    public class CitiesSuggestionProvider : ICitiesSuggestionProvider
    {
        private readonly YandexGeocoder geocoder = new YandexGeocoder();
        private readonly YandexSuggester suggester = new YandexSuggester();

        public IEnumerable GetSuggestions(string filter)
        {
            IEnumerable<string> suggestions = this.suggester.SuggestAsync(filter).GetAwaiter().GetResult().Take(5);

            GeoObjectCollection objects = new GeoObjectCollection(suggestions.AsParallel().SelectMany(elem => this.geocoder.GeocodeAsync(elem, 5).GetAwaiter().GetResult()));

            return objects.Where(geo => geo.GeocoderMetaData.Kind == GeoObjectKind.Locality && !string.IsNullOrEmpty(geo.ToString())).Distinct().ToList().Distinct();
        }

        public GeoObject SelectedCity { get; set; }
    }
}