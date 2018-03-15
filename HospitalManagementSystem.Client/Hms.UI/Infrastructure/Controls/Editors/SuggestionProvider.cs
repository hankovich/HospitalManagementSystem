namespace Hms.UI.Infrastructure.Controls.Editors
{
    using System;
    using System.Collections;

    public class SuggestionProvider : ISuggestionProvider
    {
        private Func<string, IEnumerable> _method;

        public SuggestionProvider()
        {
        }

        public SuggestionProvider(Func<string, IEnumerable> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            this._method = method;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return this._method(filter);
        }
    }
}
