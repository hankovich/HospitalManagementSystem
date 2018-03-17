namespace Hms.UI.Infrastructure.Providers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Hms.UI.Infrastructure.Helpers;

    public class BuildingsSuggestionProvider : IBuildingsSuggestionProvider
    {
        private readonly YandexGeocoder geocoder = new YandexGeocoder();
        private readonly YandexSuggester suggester = new YandexSuggester();

        public BuildingsSuggestionProvider(IStreetsSuggestionProvider streetsSuggestionProvider)
        {
            this.StreetsSuggestionProvider = streetsSuggestionProvider;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            if (this.StreetsSuggestionProvider.SelectedStreet == null)
            {
                return Enumerable.Empty<object>();
            }

            GeoObject street = this.StreetsSuggestionProvider.SelectedStreet;

            IEnumerable<string> suggestions = this.suggester.SuggestAsync(this.BuildFilter(street, filter), LangType.RU).GetAwaiter().GetResult().Take(100);

            GeoObjectCollection objects = new GeoObjectCollection(suggestions.AsParallel().SelectMany(elem => this.geocoder.GeocodeAsync(elem, 15).GetAwaiter().GetResult()));

            return objects.Where(o => this.IsBuildingOnCity(o, street)).Distinct().ToList();
        }

        private bool IsBuildingOnCity(GeoObject data, GeoObject street)
        {
            if (data.GeocoderMetaData.Kind == GeoObjectKind.Locality || data.GeocoderMetaData.Kind == GeoObjectKind.House)
            {
                if (!string.IsNullOrEmpty(data.ToString())
                    && data.GeocoderMetaData.Address.Locality == street.GeocoderMetaData.Address.Locality
                    && data.GeocoderMetaData.Address.Province == street.GeocoderMetaData.Address.Province
                    && data.GeocoderMetaData.Address?.Street == street.GeocoderMetaData.Address.Street)
                {
                    return true;
                }
            }

            return false;
        }

        private string BuildFilter(GeoObject city, string filter)
        {
            return $"{city.GeocoderMetaData.Address.Locality} {city.GeocoderMetaData.Address.Street} {filter}";
        }

        public IStreetsSuggestionProvider StreetsSuggestionProvider { get; }

        public GeoObject SelectedBuilding { get; set; }
    }
}