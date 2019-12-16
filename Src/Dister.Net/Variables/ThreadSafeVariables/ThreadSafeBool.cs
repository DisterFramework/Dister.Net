namespace Dister.Net.Variables.ThreadSafeVariables
{
    public class ThreadSafeBool : ThreadSafeVariable<bool>
    {
        public ThreadSafeBool(bool value) : base(value)
        {
        }

        public void Flip()
            => Set(!TypeValue);

        public static implicit operator ThreadSafeBool(bool value)
            => new ThreadSafeBool(value);
    }
}
