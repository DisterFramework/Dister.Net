using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Exceptions.VariablesExceptions.DisterQueueExceptions
{
    public class QueueNotExistException : Exception
    {
        public QueueNotExistException(string message) : base(message)
        {
        }
    }
}
