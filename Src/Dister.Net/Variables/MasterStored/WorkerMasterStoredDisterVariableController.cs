using Dister.Net.Communication.Message;
using System;

namespace Dister.Net.Variables.MasterStored
{
    public class WorkerMasterStoredDisterVariableController<T> : DisterVariablesController<T>
    {
        internal override void AddQueue(string name) => throw new System.NotImplementedException();
        internal override void AddQueue(string name, object[] values) => throw new NotImplementedException();

        internal override Maybe<TV> Dequeue<TV>(string name)
        {
            var packet = new MessagePacket
            {
                Topic = name,
                Type = MessageType.Dequeue
            };
            return service.Communicator.GetResponse<TV>(packet);
        }
        internal override void Enqueue(string name, object value)
        {
            var packet = new MessagePacket
            {
                Topic = name,
                Content = service.Serializer.Serialize(value),
                Type = MessageType.Enqueue
            };
            service.Communicator.SendMessage(packet);
        }

        internal override Maybe<TV> GetDisterVariable<TV>(string name)
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
