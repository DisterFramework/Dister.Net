using System;
using Dister.Net.Communication;
using Dister.Net.Communication.Message;
using Dister.Net.Exceptions.Handling;
using Dister.Net.Logs;
using Dister.Net.Serialization;
using Dister.Net.Variables.DiserVariables;

namespace Dister.Net.Service
{
    public abstract class DisterService<T>
    {
        internal Communicator<T> Communicator { get; set; }
        internal ISerializer Serializer { get; set; }
        internal bool InLoop { get; set; }
        internal MessageHandlers<T> MessageHandlers { get; set; } = new MessageHandlers<T>();
        internal ExceptionHanlders<T> ExceptionHanlders { get; set; } = new ExceptionHanlders<T>();
        internal DisterVariablesController<T> DisterVariablesController { get; set; }
        public LogAggregator<T> LogAggregator { get; internal set; }
        public abstract void Run();
        internal void Start()
        {
            do
            {
                try
                {
                    Run();
                }
                catch (Exception ex)
                {
                    if (!ExceptionHanlders.Handle(ex))
                    {
                        throw;
                    }
                }
            } while (InLoop);
        }
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

        public DisterVariable<TV, T> DisterVariable<TV>(string name)
            => new DisterVariable<TV, T>(name, DisterVariablesController);
        public DisterQueue<TV, T> DisterQueue<TV>(string name)
            => new DisterQueue<TV, T>(name, DisterVariablesController);
        public DisterDictionary<TK, TV, T> DisterDictionary<TK, TV>(string name)
            => new DisterDictionary<TK, TV, T>(name, DisterVariablesController);
    }
}
