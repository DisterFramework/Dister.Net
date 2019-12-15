using Dister.Net.Communication.Message;

namespace Dister.Net.Variables.MasterStored
{
    public class WorkerMasterStoredDisterVariableController<T> : DisterVariablesController<T>
    {
        internal override TV GetDisterVariable<TV>(string name)
        {
            var packet = new MessagePacket
            {
                Topic = name,
                Type = MessageType.VariableGet
            };
            return service.Communicator.GetResponse<TV>(packet);
        }

        internal override void SetDisterVariable(string name, object value)
        {
            var packet = new MessagePacket
            {
                Topic = name,
                Type = MessageType.VariableSet,
                Content = service.Serializer.Serialize(value)
            };
            service.Communicator.SendMessage(packet);
        }
    }
}
