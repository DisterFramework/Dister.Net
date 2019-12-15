using System;
using System.Linq;
using Dister.Net.Exceptions.VariablesExceptions;

namespace Dister.Net.Variables
{
    public class Maybe<T>
    {
        internal T[] Content { get; }

        private Maybe(T[] content)
        {
            Content = content;
        }

        public static Maybe<T> Some(T value) =>
            new Maybe<T>(new[] { value });

        public static Maybe<T> None() =>
            new Maybe<T>(new T[0]);

        public bool IsSome => Content.Length > 0;
        public bool IsNone => Content.Length == 0;

        public T Value => IsSome ? Content.First() : throw new NoneMaybeException();
    }
}
