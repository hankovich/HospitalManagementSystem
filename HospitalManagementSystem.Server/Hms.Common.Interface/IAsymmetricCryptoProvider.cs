namespace Hms.Common.Interface
{
    using System.Threading.Tasks;

    public interface IAsymmetricCryptoProvider
    {
        Task<byte[]> EncryptBytesAsync(byte[] message, byte[] key);

        Task<byte[]> DecryptBytesAsync(byte[] message, byte[] key);

        byte[] GenerateKey();
    }
}