using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Communication;
using Dister.Net.Communication.Message;
using Dister.Net.Serialization;
using Dister.Net.Variables;

namespace Dister.Net.Service
{
    public abstract class DisterService<T>
    {
        internal Communicator<T> Communicator { get; set; }
        internal ISerializer Serializer { get; set; }
        internal bool InLoop { get; set; }
        internal MessageHandlers<T> MessageHandlers { get; set; }
        internal DisterVariablesController<T> DisterVariablesController { get; set; }
        public abstract void Run();
        public void SendMessage(string topic, object o)
        {
            var packet = new MessagePacket
            {
                Content = Serializer.Serialize(o),
                Topic = topic,
                Type = MessageType.NoResponseRequest
            };
            Communicator.SendMessage(packet);
        }
        public Maybe<TM> GetResponse<TM>(string topic, object o) where TM : class
        {
            var packet = new MessagePacket
            {
                Content = Serializer.Serialize(o),
                Topic = topic,
                Type = MessageType.ResponseRequest
            };
            return Communicator.GetResponse<TM>(packet);
        }

        public DisterVariable<TV, T> GetDisterVariable<TV>(string name)
            => new DisterVariable<TV, T>(name, DisterVariablesController);
        public DisterQueue<TV, T> GetDisterQueue<TV>(string name)
            => new DisterQueue<TV, T>(name, DisterVariablesController);
    }
}
