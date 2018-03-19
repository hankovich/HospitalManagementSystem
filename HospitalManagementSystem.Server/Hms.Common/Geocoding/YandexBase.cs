namespace Hms.Common.Geocoding
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Geocoding;

    public class YandexBase
    {
        protected string StringEncode(string location)
        {
            return location.Replace(" ", "+").Replace("&", string.Empty).Replace("?", string.Empty);
        }

        protected string LangTypeToStr(LangType lang)
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

        protected async Task<string> DownloadStringAsync(string url)
        {
            using (var client = new HttpClient())
            using (var message = await client.GetAsync(url))
            {
                return await message.Content.ReadAsStringAsync();
            }
        }

        protected string BuildGeoBound(GeoBound geoBound, bool rspn = false)
        {
            return
                $"&bbox={geoBound.LowerCorner.Longitude},{geoBound.LowerCorner.Latitude}~{geoBound.UpperCorner.Longitude},{geoBound.UpperCorner.Latitude}&rspn={(rspn ? 1 : 0)}";
        }
    }
}