namespace Hms.UI.Infrastructure.Controls.Editors
{
    using System.Collections;
    using System.Threading.Tasks;

    public interface ISuggestionProvider
    {
        Task<IEnumerable> GetSuggestionsAsync(string filter);
    }
}
