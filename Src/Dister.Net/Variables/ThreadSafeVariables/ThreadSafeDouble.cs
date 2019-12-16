namespace Dister.Net.Variables.ThreadSafeVariables
{
    public class ThreadSafeDouble : ThreadSafeVariable<double>
    {
        public ThreadSafeDouble(double value) : base(value)
        {
        }

        public void Add(double value)
            => Set(TypeValue + value);
        public void Sub(double value)
            => Set(TypeValue - value);

        public static implicit operator ThreadSafeDouble(double value)
            => new ThreadSafeDouble(value);
    }
}
