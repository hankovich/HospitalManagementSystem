namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Models;

    public interface IGadgetKeysService
    {
        Task<byte[]> GetGadgetRoundKeyAsync(string identifier);

        Task<string> GetGadgetClientSecretAsync(string identifier);

        Task<KeysInfoModel> GetGadgetKeysInfoAsync(string gadgetIdentifier, string clientSecret);

        Task SetGadgetRoundKeyAsync(string identifier, string clientSecret, byte[] roundKey);

        Task SetGadgetPublicKeyAsync(string identifier, string clientSecret, byte[] publicKey);

        Task IncrementGadgetRoundKeySentTimesAsync(string gadgetIdentifier, string clientSecret);
    }
}
