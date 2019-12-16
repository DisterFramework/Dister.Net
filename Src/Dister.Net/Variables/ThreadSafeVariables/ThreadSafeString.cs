namespace Dister.Net.Variables.ThreadSafeVariables
{
    public class ThreadSafeString : ThreadSafeVariable<string>
    {
        public ThreadSafeString(string value) : base(value)
        {
        }

        public void Append(string value)
            => Set(TypeValue + value);

        public static implicit operator ThreadSafeString(string value)
            => new ThreadSafeString(value);
    }
}
