namespace Dister.Net.Exceptions.CommunicatorExceptions
{
    public class ConnectionClosedException : DisterException
    {
        public ConnectionClosedException(string message) : base(message)
        {
        }
    }
}
