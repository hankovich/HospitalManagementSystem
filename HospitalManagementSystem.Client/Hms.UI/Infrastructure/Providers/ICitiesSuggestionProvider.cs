namespace Hms.UI.Infrastructure.Providers
{
    using Hms.UI.Infrastructure.Controls.Editors;

    public interface ICitiesSuggestionProvider : ISuggestionProvider
    {
        string SelectedCity { get; set; }
    }
}