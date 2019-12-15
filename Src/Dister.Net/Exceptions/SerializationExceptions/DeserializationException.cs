using System;

namespace Dister.Net.Exceptions.SerializationExceptions
{
    public class DeserializationException : DisterException
    {
        public DeserializationException(string message = "Deserialization failed") : base(message)
        {
        }

        public DeserializationException(Exception innerException, string message = "Deserialziation failed") : base(message, innerException)
        {
        }
    }
}
