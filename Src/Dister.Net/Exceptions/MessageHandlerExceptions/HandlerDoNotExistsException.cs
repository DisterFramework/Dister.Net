namespace Dister.Net.Exceptions.MessageHandlerExceptions
{
    public class HandlerDoNotExistsException : DisterException
    {
        public HandlerDoNotExistsException(string message) : base(message)
        {
        }
    }
}
