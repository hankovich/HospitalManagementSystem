namespace Hms.UI.Infrastructure.Providers
{
    using Hms.Common.Interface.Geocoding;
    using Hms.UI.Infrastructure.Controls.Editors;

    public interface IBuildingsSuggestionProvider : ISuggestionProvider
    {
        IStreetsSuggestionProvider StreetsSuggestionProvider { get; }

        GeoObject SelectedBuilding { get; set; }
    }
}