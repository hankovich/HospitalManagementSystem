namespace Hms.DataServices.Infrasructure
{
    using Hms.Common.Interface.Models;
    using Hms.DataServices.Interface.Infrastructure;

    public class ClientStateModel : IClientStateModel
    {
        public LoginModel AuthInfo { get; set; } = new LoginModel();

        public string Identifier { get; set; }

        public string ClientSecret { get; set; }

        public byte[] PrivateKey { get; set; }

        public byte[] RoundKey { get; set; }
    }
}