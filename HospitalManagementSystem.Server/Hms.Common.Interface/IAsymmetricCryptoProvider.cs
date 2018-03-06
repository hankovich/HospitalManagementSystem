namespace Hms.Common.Interface
{
    using System.Threading.Tasks;

    public interface IAsymmetricCryptoProvider
    {
        Task<byte[]> EncryptBytesAsync(byte[] message, byte[] publicKey);

        Task<byte[]> DecryptBytesAsync(byte[] message, byte[] privateKey);

        byte[] GeneratePrivateKey();

        byte[] GetPublicKey(byte[] privateKey);
    }
}