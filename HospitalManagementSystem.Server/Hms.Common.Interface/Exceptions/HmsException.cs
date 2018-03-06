namespace Hms.Common.Interface.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class HmsException : Exception
    {
        public HmsException()
        {
        }

        public HmsException(string message) : base(message)
        {
        }

        public HmsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HmsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
