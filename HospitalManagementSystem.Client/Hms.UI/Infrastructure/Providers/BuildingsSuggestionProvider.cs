namespace Hms.UI.Infrastructure.Providers
{
    using System.Collections;
    using System.Threading.Tasks;

    public class BuildingsSuggestionProvider : IBuildingsSuggestionProvider
    {
        public BuildingsSuggestionProvider(IStreetsSuggestionProvider streetsSuggestionProvider)
        {
            this.StreetsSuggestionProvider = streetsSuggestionProvider;
        }

        public Task<IEnumerable> GetSuggestionsAsync(string filter)
        {
            throw new System.NotImplementedException();
        }

        public IStreetsSuggestionProvider StreetsSuggestionProvider { get; }

        public string SelectedBuilding { get; set; }
    }
}