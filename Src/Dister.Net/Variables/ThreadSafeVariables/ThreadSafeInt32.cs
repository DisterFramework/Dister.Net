using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Variables.ThreadSafeVariables
{
    public class ThreadSafeInt32 : ThreadSafeVariable<int>
    {
        public ThreadSafeInt32(int value) : base(value)
        {
        }

        public void Add(int value)
            => Set(TypeValue + value);
        public void Sub(int value)
            => Set(TypeValue - value);
        public void Increment()
            => Add(1);

        public static implicit operator ThreadSafeInt32(int value)
            => new ThreadSafeInt32(value);
    }
}
