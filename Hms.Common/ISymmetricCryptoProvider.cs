namespace Hms.Common
{
    using System.Threading.Tasks;

    public interface ISymmetricCryptoProvider
    {
        Task<byte[]> EncryptBytesAsync(byte[] message, byte[] key, byte[] iv);

        Task<byte[]> DecryptBytesAsync(byte[] message, byte[] key, byte[] iv);
    }
}
