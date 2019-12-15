using System;

namespace Dister.Net.Exceptions.VariablesExceptions.DisterQueueExceptions
{
    public class EmptyQueueException : Exception
    {
        public EmptyQueueException(string message) : base(message)
        {
        }
    }
}
