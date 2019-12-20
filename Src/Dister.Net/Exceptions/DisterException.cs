using System;
using System.Runtime.Serialization;

namespace Dister.Net.Exceptions
{
    /// <summary>
    /// Base DisterException
    /// </summary>
    public class DisterException : Exception
    {
        public DisterException()
        {
        }

        public DisterException(string message) : base(message)
        {
        }

        public DisterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DisterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
