namespace Hms.UI.Infrastructure.Providers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Hms.Common.Interface.Geocoding;
    using Hms.UI.Infrastructure.Controls.Editors;

    public class GeoSuggestionProvider : ISuggestionProvider
    {
        private readonly CitiesSuggestionProvider citiesSuggestionProvider;
        private readonly StreetsSuggestionProvider streetsSuggestionProvider;
        private readonly BuildingsSuggestionProvider buildingsSuggestionProvider;

        public GeoSuggestionProvider(IGeoSuggester geoSuggester, IGeocoder geocoder)
        {
            this.citiesSuggestionProvider = new CitiesSuggestionProvider(geoSuggester, geocoder);
            this.streetsSuggestionProvider = new StreetsSuggestionProvider(geoSuggester, geocoder);
            this.buildingsSuggestionProvider = new BuildingsSuggestionProvider(geoSuggester, geocoder);
        }

        public IEnumerable GetSuggestions(string filter, object parameter)
        {
            if (parameter == null)
            {
                return this.citiesSuggestionProvider.GetSuggestions(filter, null);
            }

            var list = parameter as List<object>;

            string geoObjectKind = list?[0] as string;
            GeoObject geoObject = list?[1] as GeoObject;

            if (geoObject != null)
            {
                if (geoObject.GeocoderMetaData.Kind == GeoObjectKind.Locality && geoObjectKind == "Street")
                {
                    return this.streetsSuggestionProvider.GetSuggestions(filter, geoObject);
                }

                if (geoObject.GeocoderMetaData.Kind == GeoObjectKind.Street && geoObjectKind == "Building")
                {
                    return this.buildingsSuggestionProvider.GetSuggestions(filter, geoObject);
                }
            }

            return null;
        }
    }
}
