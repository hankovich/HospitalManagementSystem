namespace Hms.UI.Infrastructure.Providers
{
    using Hms.UI.Infrastructure.Controls.Editors;

    public interface IBuildingsSuggestionProvider : ISuggestionProvider
    {
        IStreetsSuggestionProvider StreetsSuggestionProvider { get; }

        string SelectedBuilding { get; set; }
    }
}