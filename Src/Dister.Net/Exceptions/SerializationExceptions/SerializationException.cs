using System;

namespace Dister.Net.Exceptions.SerializationExceptions
{
    public class SerializationException : DisterException
    {
        public SerializationException(string message = "Serialization failed") : base(message)
        {
        }

        public SerializationException(Exception innerException, string message = "Serialziation failed") : base(message, innerException)
        {
        }
    }
}
