namespace Hms.UI.Infrastructure.Providers
{
    using Hms.Common.Interface.Geocoding;
    using Hms.UI.Infrastructure.Controls.Editors;

    public interface IStreetsSuggestionProvider : ISuggestionProvider
    {
        ICitiesSuggestionProvider CitiesSuggestionProvider { get; }

        GeoObject SelectedStreet { get; set; }
    }
}