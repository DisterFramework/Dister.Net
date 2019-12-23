namespace Dister.Net.Exceptions.ServiceBuilderExceptions
{
    public class DisterVariableControllerNotSetException : DisterException
    {
        public DisterVariableControllerNotSetException(string message = "DisterVariableController is not set. Use WithDisterVariableController method to specify DisterVariableController") : base(message)
        {
        }
    }
}
