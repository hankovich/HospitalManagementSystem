namespace Hms.Common.ElGamal
{
    using System;
    using System.Numerics;
    using System.Text;

    using Newtonsoft.Json;

    public struct ElGamalKey
    {
        public BigInteger P { get; set; }

        public BigInteger G { get; set; }

        public BigInteger Y { get; set; }

        public BigInteger X { get; set; }

        public int GetPlaintextBlockSize()
        {
            return (this.P.BitCount() - 1) / 8;
        }

        public int GetCiphertextBlockSize()
        {
            return (((this.P.BitCount() + 7) / 8) * 2) + 2;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        public static bool TryParseBytes(byte[] bytes, out ElGamalKey key)
        {
            try
            {
                if (bytes == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                key = JsonConvert.DeserializeObject<ElGamalKey>(Encoding.UTF8.GetString(bytes));
                return true;
            }
            catch (Exception e)
            {
                key = default(ElGamalKey);
                return false;
            }
        }
    }
}