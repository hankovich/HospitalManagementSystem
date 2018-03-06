namespace Hms.Services
{
    using System;
    using System.Threading.Tasks;

    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class GadgetKeysService : IGadgetKeysService
    {
        public GadgetKeysService(IGadgetKeysInfoRepository gadgetKeysInfoRepository)
        {
            if (gadgetKeysInfoRepository == null)
            {
                throw new ArgumentNullException(nameof(gadgetKeysInfoRepository));
            }

            this.GadgetKeysInfoRepository = gadgetKeysInfoRepository;
        }

        public IGadgetKeysInfoRepository GadgetKeysInfoRepository { get; set; }

        public async Task<byte[]> GetGadgetRoundKeyAsync(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(identifier));
            }

            return await this.GadgetKeysInfoRepository.GetGadgetRoundKeyAsync(identifier);
        }

        public async Task<string> GetGadgetClientSecretAsync(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(identifier));
            }

            return await this.GadgetKeysInfoRepository.GetGadgetClientSecretAsync(identifier);
        }

        public async Task SetGadgetRoundKeyAsync(string identifier, string clientSecret, byte[] roundKey)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException($"{nameof(identifier)} is null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentException($"{nameof(clientSecret)} is null or whitespace");
            }

            if (roundKey?.Length == 0)
            {
                throw new ArgumentException($"{nameof(roundKey)} is null or empty");
            }

            await this.GadgetKeysInfoRepository.SetGadgetRoundKey(identifier, clientSecret, roundKey);
        }

        public async Task SetGadgetPublicKeyAsync(string identifier, string clientSecret, byte[] publicKey)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException($"{nameof(identifier)} is null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentException($"{nameof(clientSecret)} is null or whitespace");
            }

            if (publicKey?.Length == 0)
            {
                throw new ArgumentException($"{nameof(publicKey)} is null or empty");
            }

            await this.GadgetKeysInfoRepository.SetGadgetPublicKeyAsync(identifier, clientSecret, publicKey);
        }
    }
}
