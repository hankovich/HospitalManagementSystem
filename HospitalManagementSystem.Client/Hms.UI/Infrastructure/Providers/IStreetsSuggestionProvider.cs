namespace Hms.UI.Infrastructure.Providers
{
    using Hms.UI.Infrastructure.Controls.Editors;

    public interface IStreetsSuggestionProvider : ISuggestionProvider
    {
        ICitiesSuggestionProvider CitiesSuggestionProvider { get; }

        string SelectedStreet { get; set; }
    }
}