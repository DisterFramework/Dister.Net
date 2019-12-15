using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Exceptions.ServiceBuilderExceptions
{
    public class SerializerNotSetException : Exception
    {
        public SerializerNotSetException(string message = "Serializer is not set. Use WithSerializer method to specify serializer") : base(message)
        {
        }
    }
}
