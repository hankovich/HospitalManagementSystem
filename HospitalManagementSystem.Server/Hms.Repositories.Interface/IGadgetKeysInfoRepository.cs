namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Models;

    public interface IGadgetKeysInfoRepository
    {
        Task<byte[]> GetGadgetRoundKeyAsync(string gadgetIdentifier);

        Task<byte[]> GetGadgetPublicKeyAsync(string gadgetIdentifier, string clientSecret);

        Task<string> GetGadgetClientSecretAsync(string gadgetIdentifier);

        Task<KeysInfoModel> GetGadgetKeysInfoAsync(string gadgetIdentifier, string clientSecret);

        Task IncrementGadgetRoundKeySentTimesAsync(string gadgetIdentifier, string clientSecret);

        Task SetGadgetPublicKeyAsync(string gadgetIdentifier, string clientSecret, byte[] publicKey);

        Task SetGadgetRoundKey(string gadgetIdentifier, string clientSecret, byte[] roundKey);
    }
}
