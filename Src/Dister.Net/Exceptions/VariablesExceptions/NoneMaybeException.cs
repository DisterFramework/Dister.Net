namespace Dister.Net.Exceptions.VariablesExceptions
{
    public class NoneMaybeException : DisterException
    {
        public NoneMaybeException(string message = "Tried to get value from None Maybe") : base(message)
        {
        }
    }
}
