namespace Hms.UI.Infrastructure.Controls.Editors
{
    using System.Collections;

    public interface ISuggestionProvider
    {
        IEnumerable GetSuggestions(string filter);
    }
}
