namespace Hms.UI.Infrastructure.Providers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Hms.Common.Interface.Geocoding;
    using Hms.UI.Infrastructure.Controls.Editors;

    public class StreetsSuggestionProvider : ISuggestionProvider
    {
        private readonly IGeoSuggester geoSuggester;

        private readonly IGeocoder geocoder;

        public StreetsSuggestionProvider(IGeoSuggester geoSuggester, IGeocoder geocoder)
        {
            this.geoSuggester = geoSuggester;
            this.geocoder = geocoder;
        }

        public IEnumerable GetSuggestions(string filter, object parameter)
        {
            GeoObject city = (GeoObject)parameter;

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
    }
}