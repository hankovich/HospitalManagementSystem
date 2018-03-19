namespace Hms.Common.Geocoding
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Geocoding;

    public class YandexGeocoder : YandexBase, IGeocoder
    {
        public const string RequestUrl =
            "http://geocode-maps.yandex.ru/1.x/?geocode={0}&format=xml&results={1}&lang={2}";

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
            return await this.GeocodeAsync(location, 10000);
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
                string.Format(RequestUrl, this.StringEncode(location), results, this.LangTypeToStr(lang))
                + (string.IsNullOrEmpty(this.Key) ? string.Empty : "&key=" + this.Key);

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
                string.Format(RequestUrl, this.StringEncode(location), results, this.LangTypeToStr(lang))
                + $"&ll={searchArea.Center.ToString("{0},{1}")}&spn={searchArea.Center.ToString("{0},{1}")}&rspn={(rspn ? 1 : 0)}"
                + (string.IsNullOrEmpty(this.Key) ? string.Empty : "&key=" + this.Key);

            return new GeoObjectCollection(await this.DownloadStringAsync(requestUlr));
        }

        /// <summary>
        /// Location determination by name, indicating the quantity of objects to return and preference language.
        /// Allows limit the search or affect the issuance result.
        /// </summary>
        /// <param name="location">Name of a geographic location.</param>
        /// <param name="results">Maximum number of objects to return.</param>
        /// <param name="lang">Preference language for describing objects.</param>
        /// <param name="geoBound">Search geographical area, affects to issuance of results.</param>
        /// <param name="rspn">Allows limit the search (true) or affect the issuance result (false - default).</param>
        /// <returns>Collection of found locations</returns>
        public async Task<GeoObjectCollection> GeocodeAsync(
            string location,
            short results,
            LangType lang,
            GeoBound geoBound,
            bool rspn = false)
        {
            string requestUlr =
                string.Format(RequestUrl, this.StringEncode(location), results, this.LangTypeToStr(lang))
                + this.BuildGeoBound(geoBound, rspn)
                + (string.IsNullOrEmpty(this.Key) ? string.Empty : "&key=" + this.Key);

            return new GeoObjectCollection(await this.DownloadStringAsync(requestUlr));
        }

        public async Task<GeoObjectCollection> ReverseGeocodeAsync(GeoPoint point)
        {
            return await this.ReverseGeocodeAsync(point, GeoObjectKind.House);
        }

        public async Task<GeoObjectCollection> ReverseGeocodeAsync(GeoPoint point, GeoObjectKind kind)
        {
            return await this.ReverseGeocodeAsync(point, GeoObjectKind.House, 10000);
        }

        public async Task<GeoObjectCollection> ReverseGeocodeAsync(GeoPoint point, GeoObjectKind kind, short results)
        {
            return await this.ReverseGeocodeAsync(point, GeoObjectKind.House, 10000, LangType.RU);
        }

        public async Task<GeoObjectCollection> ReverseGeocodeAsync(
            GeoPoint point,
            GeoObjectKind kind,
            short results,
            LangType lang)
        {
            string requestUlr =
                string.Format(RequestUrl, $"{point.Longittude},{point.Latitude}", results, this.LangTypeToStr(lang))
                + $"&kind={kind.ToString().ToLower()}"
                + (string.IsNullOrEmpty(this.Key) ? string.Empty : "&key=" + this.Key);

            return new GeoObjectCollection(await this.DownloadStringAsync(requestUlr));
        }
    }
}