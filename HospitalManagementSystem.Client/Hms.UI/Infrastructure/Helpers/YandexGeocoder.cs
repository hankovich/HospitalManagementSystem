namespace Hms.UI.Infrastructure.Providers
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public class YandexGeocoder
    {
        public const string RequestUrl = "http://geocode-maps.yandex.ru/1.x/?geocode={0}&format=xml&results={1}&lang={2}";

        public YandexGeocoder()
        {
            this.Key = string.Empty;
        }

        /// <summary>
        /// Yandex Maps API-key (not necessarily)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Location determination by geo object name.
        /// </summary>
        /// <param name="location">Name of a geographic location.</param>
        /// <example>Geocode("Moscow");</example>
        /// <returns>Collection of found locations</returns>
        public async Task<GeoObjectCollection> GeocodeAsync(string location)
        {
            return await this.GeocodeAsync(location, 150);
        }

        /// <summary>
        /// Location determination by name, indicating the quantity of objects to return.
        /// </summary>
        /// <param name="location">Name of a geographic location.</param>
        /// <param name="results">Maximum number of objects to return.</param>
        /// <example>Geocode("Moscow", 10);</example>
        /// <returns>Collection of found locations</returns>
        public async Task<GeoObjectCollection> GeocodeAsync(string location, short results)
        {
            return await this.GeocodeAsync(location, results, LangType.RU);
        }

        /// <summary>
        /// Location determination by name, indicating the quantity of objects to return and preference language.
        /// </summary>
        /// <param name="location">Name of a geographic location.</param>
        /// <param name="results">Maximum number of objects to return.</param>
        /// <param name="lang">Preference language for describing objects.</param>
        /// <example>Geocode("Moscow", 10, LangType.en_US);</example>
        /// <returns>Collection of found locations</returns>
        public async Task<GeoObjectCollection> GeocodeAsync(string location, short results, LangType lang)
        {
            string requestUlr =
                string.Format(RequestUrl, this.StringMakeValid(location), results, LangTypeToStr(lang)) +
                (string.IsNullOrEmpty(this.Key) ? string.Empty : "&key=" + this.Key);

            return new GeoObjectCollection(await this.DownloadStringAsync(requestUlr));
        }

        /// <summary>
        /// Location determination by name, indicating the quantity of objects to return and preference language.
        /// Allows limit the search or affect the issuance result.
        /// </summary>
        /// <param name="location">Name of a geographic location.</param>
        /// <param name="results">Maximum number of objects to return.</param>
        /// <param name="lang">Preference language for describing objects.</param>
        /// <param name="searchArea">Search geographical area, affects to issuance of results.</param>
        /// <param name="rspn">Allows limit the search (true) or affect the issuance result (false - default).</param>
        /// <returns>Collection of found locations</returns>
        public async Task<GeoObjectCollection> GeocodeAsync(
            string location,
            short results,
            LangType lang,
            SearchArea searchArea,
            bool rspn = false)
        {
            string requestUlr =
                string.Format(RequestUrl, this.StringMakeValid(location), results, LangTypeToStr(lang))
                + $"&ll={searchArea.longLat.ToString("{0},{1}")}&spn={searchArea.spread.ToString("{0},{1}")}&rspn={(rspn ? 1 : 0)}"
                + (string.IsNullOrEmpty(this.Key) ? string.Empty : "&key=" + this.Key);

            return new GeoObjectCollection(await this.DownloadStringAsync(requestUlr));
        }

        private string StringMakeValid(string location)
        {
            return location.Replace(" ", "+").Replace("&", string.Empty).Replace("?", string.Empty);
        }

        private static string LangTypeToStr(LangType lang)
        {
            switch (lang)
            {
                case LangType.RU: return "ru-RU";
                case LangType.UA: return "uk-UA";
                case LangType.BY: return "be-BY";
                case LangType.US: return "en-US";
                case LangType.BR: return "en-BR";
                case LangType.TR: return "tr-TR";
                default: return "ru-RU";
            }
        }

        private async Task<string> DownloadStringAsync(string url)
        {
            using (var client = new HttpClient())
            using (var message = await client.GetAsync(url))
            {
                return await message.Content.ReadAsStringAsync();
            }
        }
    }
}