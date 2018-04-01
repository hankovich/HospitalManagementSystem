namespace Hms.UI.Infrastructure.Providers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Hms.Common.Interface.Geocoding;
    using Hms.UI.Infrastructure.Controls.Editors;

    public class CitiesSuggestionProvider : ISuggestionProvider
    {
        public CitiesSuggestionProvider(IGeoSuggester geoSuggester, IGeocoder geocoder)
        {
            this.GeoSuggester = geoSuggester;
            this.Geocoder = geocoder;
        }

        public IGeocoder Geocoder { get; }

        public IGeoSuggester GeoSuggester { get; }

        public IEnumerable GetSuggestions(string filter, object parameter)
        {
            IEnumerable<string> suggestions = this.GeoSuggester.SuggestAsync(filter).GetAwaiter().GetResult().Take(5);

            GeoObjectCollection objects = new GeoObjectCollection(suggestions.AsParallel().SelectMany(elem => this.Geocoder.GeocodeAsync(elem, 5).GetAwaiter().GetResult()));

            return objects.Where(geo => geo.GeocoderMetaData.Kind == GeoObjectKind.Locality && !string.IsNullOrEmpty(geo.ToString())).Distinct().ToList().Distinct();
        }
    }
}