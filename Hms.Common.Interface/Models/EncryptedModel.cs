namespace Hms.Common.Interface.Models
{
    public class EncryptedModel<T>
    {
        public T Value { get; set; }

        public byte[] Iv { get; set; }
    }
}
