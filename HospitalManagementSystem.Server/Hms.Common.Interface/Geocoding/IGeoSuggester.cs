namespace Hms.Common.Interface.Geocoding
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGeoSuggester
    {
        Task<IEnumerable<string>> SuggestAsync(string part);

        Task<IEnumerable<string>> SuggestAsync(string part, LangType langType);

        Task<IEnumerable<string>> SuggestAsync(string part, LangType langType, GeoBound geoBound, bool rspn = false);
    }
}