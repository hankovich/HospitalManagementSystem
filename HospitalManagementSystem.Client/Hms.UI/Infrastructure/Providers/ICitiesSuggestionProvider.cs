namespace Hms.UI.Infrastructure.Providers
{
    using Hms.Common.Interface.Geocoding;
    using Hms.UI.Infrastructure.Controls.Editors;

    public interface ICitiesSuggestionProvider : ISuggestionProvider
    {
        IGeocoder Geocoder { get; }

        IGeoSuggester GeoSuggester { get; }

        GeoObject SelectedCity { get; set; }
    }
}