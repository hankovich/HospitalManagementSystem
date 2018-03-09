namespace Hms.Common.Interface.Models
{
    public class AuthHeaderModel
    {
        public string Indentifier { get; set; }

        public string ClientSecret { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public byte[] Iv { get; set; }
    }
}
