namespace Hms.Services.Interface
{
    public class ServerResponse<T>
    {
        public int StatusCode { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public T Content { get; set; }
    }
}
