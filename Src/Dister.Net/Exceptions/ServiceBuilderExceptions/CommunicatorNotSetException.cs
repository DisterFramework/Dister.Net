using System;

namespace Dister.Net.Exceptions.ServiceBuilderExceptions
{
    public class CommunicatorNotSetException : Exception
    {
        public CommunicatorNotSetException(string message = "Communicator is not set. Use WithCommunicator method to specify communicator") : base(message)
        {
        }
    }
}
