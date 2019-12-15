using System;

namespace Dister.Net.Exceptions.ServiceBuilderExceptions
{
    public class DisterVariableControllerNotSetException : Exception
    {
        public DisterVariableControllerNotSetException(string message = "DisterVariableController is not set. Use WithDisterVariableController method to specify DisterVariableController") : base(message)
        {
        }
    }
}
