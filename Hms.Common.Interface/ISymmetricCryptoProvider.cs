namespace Hms.Common.Interface
{
    using System.Threading.Tasks;

    public interface ISymmetricCryptoProvider
    {
        int KeySize { get; }

        Task<byte[]> EncryptBytesAsync(byte[] message, byte[] key, byte[] iv);

        Task<byte[]> DecryptBytesAsync(byte[] message, byte[] key, byte[] iv);

        byte[] GenerateKey();

        byte[] GenerateIv();
    }
}
