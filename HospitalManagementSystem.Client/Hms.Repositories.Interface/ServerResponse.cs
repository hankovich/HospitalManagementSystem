namespace Hms.Repositories.Interface
{
    public class ServerResponse
    {
        public int StatusCode { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public string Content { get; set; }
    }
}
