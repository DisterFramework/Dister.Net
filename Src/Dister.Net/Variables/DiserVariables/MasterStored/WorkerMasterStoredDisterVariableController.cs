using System;
using System.Collections.Generic;
using Dister.Net.Communication.Message;

namespace Dister.Net.Variables.DiserVariables.MasterStored
{
    public class WorkerMasterStoredDisterVariableController<T> : DisterVariablesController<T>
    {
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

        internal override void AddQueue(string name) => throw new NotImplementedException();
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

        internal override void AddDictionary(string name) => throw new NotImplementedException();
        internal override void AddDictionary(string name, Dictionary<object, object> values) => throw new NotImplementedException();
        internal override Maybe<TV> GetFromDictionary<TV>(string name, object key)
        {
            var packet = new MessagePacket
            {
                Topic = name,
                Content = service.Serializer.Serialize(key),
                Type = MessageType.DictionaryGet
            };
            return service.Communicator.GetResponse<TV>(packet);
        }
        internal override void SetInDictionary(string name, object key, object value)
        {
            var packet = new MessagePacket
            {
                Topic = name,
                Content = service.Serializer.Serialize(new KeyValuePair<object, object>(key, value)),
                Type = MessageType.DictionarySet
            };
            service.Communicator.SendMessage(packet);
        }
    }
}
