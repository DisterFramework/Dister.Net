using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Exceptions.VariablesExceptions
{
    public class NoneMaybeException : Exception
    {
        public NoneMaybeException(string message = "Tried to get value from None Maybe") : base(message)
        {
        }
    }
}
