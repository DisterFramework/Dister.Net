using System;
using Dister.Net.Communication;
using Dister.Net.Communication.Message;
using Dister.Net.Exceptions.Handling;
using Dister.Net.Logs;
using Dister.Net.Serialization;
using Dister.Net.Variables.DiserVariables;
using Dister.Net.WorkChunks;

namespace Dister.Net.Service
{
    /// <summary>
    /// Base class of every DisterServices
    /// </summary>
    /// <typeparam name="T">Type of service</typeparam>
    public abstract class DisterService<T>
    {
        internal Communicator<T> Communicator { get; set; }
        internal ISerializer Serializer { get; set; }
        internal bool InLoop { get; set; }
        internal MessageHandlers<T> MessageHandlers { get; set; } = new MessageHandlers<T>();
        internal ExceptionHanlders<T> ExceptionHanlders { get; set; } = new ExceptionHanlders<T>();
        internal WorkChunkGenerators<T> WorkChunkGenerators { get; set; } = new WorkChunkGenerators<T>();
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

        /// <summary>
        /// Send message through <see cref="Communicator"/>
        /// </summary>
        /// <param name="topic">Topic of message</param>
        /// <param name="o">Content of message</param>
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
        /// <summary>
        /// Get response from <see cref="Communicator"/>
        /// </summary>
        /// <typeparam name="TM">Type of response</typeparam>
        /// <param name="topic">Topic of request message</param>
        /// <param name="o">Content of message request</param>
        /// <returns>Communicator response</returns>
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

        /// <summary>
        /// <see cref="DisterVariable{TV, TS}"/> handler
        /// </summary>
        /// <typeparam name="TV">Variable type</typeparam>
        /// <param name="name">variable name</param>
        /// <returns>Handler to variable</returns>
        public DisterVariable<TV, T> DisterVariable<TV>(string name)
            => new DisterVariable<TV, T>(name, DisterVariablesController);
        /// <summary>
        /// <see cref="DisterQueue{TV, TS}"/> handler
        /// </summary>
        /// <typeparam name="TV">Queue type</typeparam>
        /// <param name="name">Queue name</param>
        /// <returns>Handler to queue</returns>
        public DisterQueue<TV, T> DisterQueue<TV>(string name)
            => new DisterQueue<TV, T>(name, DisterVariablesController);
        /// <summary>
        /// <see cref="DisterDictionary{TK, TV, TS}"/> handler
        /// </summary>
        /// <typeparam name="TK">Type of key</typeparam>
        /// <typeparam name="TV">Type of value</typeparam>
        /// <param name="name">Dictionary name</param>
        /// <returns>Handler to dictionary</returns>
        public DisterDictionary<TK, TV, T> DisterDictionary<TK, TV>(string name)
            => new DisterDictionary<TK, TV, T>(name, DisterVariablesController);
        public Maybe<WorkChunk<T, TV>> GetWorkChunk<TV>(string name)
        {
            var packet = new MessagePacket
            {
                Topic = name,
                Type = MessageType.WorkChunkRequest
            };
            var response = Communicator.GetResponse<TV>(packet);
            if (response.IsNone) return Maybe<WorkChunk<T, TV>>.None();
            var workChunk = new WorkChunk<T, TV>
            {
                Id = packet.Id,
                disterService = this,
                Value = response.Value,
                Name = name
            };
            return Maybe<WorkChunk<T,TV>>.Some(workChunk);
        }
    }
}
