namespace Hms.UI.Infrastructure.Providers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Hms.Common.Interface.Geocoding;
    using Hms.UI.Infrastructure.Controls.Editors;

    public class BuildingsSuggestionProvider : ISuggestionProvider
    {
        public BuildingsSuggestionProvider(IGeoSuggester geoSuggester, IGeocoder geocoder)
        {
            this.GeoSuggester = geoSuggester;
            this.Geocoder = geocoder;
        }

        public IGeocoder Geocoder { get; }

        public IGeoSuggester GeoSuggester { get; }

        public IEnumerable GetSuggestions(string filter, object parameter)
        {
            GeoObject street = (GeoObject)parameter;

            IEnumerable<string> suggestions = this.GeoSuggester.SuggestAsync(this.BuildFilter(street, filter)).GetAwaiter().GetResult().Take(100);

            GeoObjectCollection objects = new GeoObjectCollection(suggestions.AsParallel().SelectMany(elem => this.Geocoder.GeocodeAsync(elem, 15).GetAwaiter().GetResult()));

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
    }
}