using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Exceptions.SerializationExceptions
{
    public class SerializationException : Exception
    {
        public SerializationException(string message = "Serialization failed") : base(message)
        {
        }

        public SerializationException(Exception innerException, string message = "Serialziation failed") : base(message, innerException)
        {
        }
    }
}
