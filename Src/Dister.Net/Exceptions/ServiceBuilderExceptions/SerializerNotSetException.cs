namespace Dister.Net.Exceptions.ServiceBuilderExceptions
{
    public class SerializerNotSetException : DisterException
    {
        public SerializerNotSetException(string message = "Serializer is not set. Use WithSerializer method to specify serializer") : base(message)
        {
        }
    }
}
