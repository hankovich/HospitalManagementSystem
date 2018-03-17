﻿namespace Hms.UI.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hms.UI.Infrastructure.Providers;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class YandexSuggester : YandexBase
    {
        private const string RequestUrl =
            "http://suggest-maps.yandex.ru/suggest-geo?lang={1}&fullpath=1&search_type=all&part={0}";

        public async Task<IEnumerable<string>> SuggestAsync(string part)
        {
            return await this.SuggestAsync(part, LangType.RU);
        }

        public async Task<IEnumerable<string>> SuggestAsync(string part, LangType langType)
        {
            string requestUrl = string.Format(RequestUrl, this.StringEncode(part), LangTypeToStr(langType));

            return await this.SuggestinRequestInternal(requestUrl);
        }

        public async Task<IEnumerable<string>> SuggestAsync(
            string part,
            LangType langType,
            GeoBound geoBound,
            bool rspn = false)
        {
            string requestUrl = string.Format(RequestUrl, this.StringEncode(part), LangTypeToStr(langType))
                                + $"&bbox={geoBound.lowerCorner.Long},{geoBound.lowerCorner.Lat}~{geoBound.upperCorner.Long},{geoBound.upperCorner.Lat}&rspn={(rspn ? 1 : 0)}";

            return await this.SuggestinRequestInternal(requestUrl);
        }

        private async Task<IEnumerable<string>> SuggestinRequestInternal(string requestUrl)
        {
            string responsePadded = await this.DownloadStringAsync(requestUrl);
            string response = responsePadded.Substring("suggest.apply(".Length).TrimEnd(')');
            var array = JsonConvert.DeserializeObject<JArray>(response)[1];

            IEnumerable<string> suggestions = array.Select(arr => (arr as JArray)?[2]?.ToString()).Skip(1);

            return suggestions;
        }
    }
}
