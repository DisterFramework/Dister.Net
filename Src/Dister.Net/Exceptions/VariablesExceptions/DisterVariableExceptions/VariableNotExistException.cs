using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Exceptions.VariablesExceptions.DisterVariableExceptions
{
    public class VariableNotExistException : Exception
    {
        public VariableNotExistException(string message) : base(message)
        {
        }
    }
}
