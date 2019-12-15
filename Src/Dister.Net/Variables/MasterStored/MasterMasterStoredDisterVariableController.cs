using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Communication.Message;

namespace Dister.Net.Variables.MasterStored
{
    public class MasterMasterStoredDisterVariableController<T> : DisterVariablesController<T>
    {
        private ConcurrentDictionary<string, object> variables = new ConcurrentDictionary<string, object>();

        internal override TV GetDisterVariable<TV>(string name)
        {
            if (variables.ContainsKey(name))
                return (TV)variables[name];
            else
                throw new KeyNotFoundException();
        }
        internal override void SetDisterVariable(string name, object value)
        {
            variables.AddOrUpdate(name, value, (x, y) => value);
        }
    }
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
