namespace Hms.Common
{
    using System.Threading.Tasks;

    public interface IAsymmetricCryptoProvider
    {
        Task<byte[]> EncryptAsync(byte[] message, byte[] key);

        Task<byte[]> DecryptAsync(byte[] message, byte[] key);

        byte[] GenerateKey();
    }
}