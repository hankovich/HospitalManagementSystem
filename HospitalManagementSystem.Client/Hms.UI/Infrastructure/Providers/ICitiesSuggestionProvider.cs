namespace Hms.UI.Infrastructure.Providers
{
    using Hms.UI.Infrastructure.Controls.Editors;

    public interface ICitiesSuggestionProvider : ISuggestionProvider
    {
        GeoObject SelectedCity { get; set; }
    }
}