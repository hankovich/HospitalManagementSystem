namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    public interface IGadgetKeysService
    {
        Task<byte[]> GetGadgetRoundKeyAsync(string identifier);

        Task<string> GetGadgetClientSecretAsync(string identifier);

        Task SetGadgetRoundKeyAsync(string identifier, string clientSecret, byte[] roundKey);

        Task SetGadgetPublicKeyAsync(string identifier, string clientSecret, byte[] publicKey);
    }
}
