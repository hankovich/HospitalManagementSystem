namespace Hms.DataServices.Interface.Infrastructure
{
    public interface IClientStateModel
    {
        string Identifier { get; }

        string ClientSecret { get; }

        byte[] PrivateKey { get; }

        byte[] RoundKey { get; }
    }
}
