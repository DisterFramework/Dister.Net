using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Exceptions.MessageHandlerExceptions
{
    public class HandlerDoNotExistsException : Exception
    {
        public HandlerDoNotExistsException(string message) : base(message)
        {
        }
    }
}
