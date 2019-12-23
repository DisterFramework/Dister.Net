using System.Threading;

namespace Dister.Net.Variables.ThreadSafeVariables
{
    public class ThreadSafeVariable<T>
    {
        protected object value;
        protected T TypeValue => (T)value;
        public ThreadSafeVariable(T value)
        {
            this.value = value;
        }
        public void Set(T value)
            => Interlocked.Exchange(ref this.value, (object)value);
        public T Get()
            => TypeValue;
        public static implicit operator ThreadSafeVariable<T>(T value)
            => new ThreadSafeVariable<T>(value);
    }
}
