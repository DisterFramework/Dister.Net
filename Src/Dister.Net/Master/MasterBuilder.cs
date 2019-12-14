using System;
using System.Threading.Tasks;
using Dister.Net.Communication.Master;
using Dister.Net.Serialization;

namespace Dister.Net.Master
{
    public class MasterBuilder<T> where T : DisterMaster<T>
    {
        readonly T master;
        MasterCommunicator<T> communicator;
        ISerializer serializer;
        bool runInLoop = false;

        internal MasterBuilder(T master)
        {
            this.master = master ?? throw new ArgumentNullException(nameof(master));
        }
        public MasterBuilder<T> WithSerializer(ISerializer serializer)
        {
            this.serializer = serializer;
            return this;
        }
        public MasterBuilder<T> WithCommunicator(MasterCommunicator<T> communicator)
        {
            this.communicator = communicator;
            return this;
        }
        public MasterBuilder<T> WithMessageHandler<TM>(string topic, Func<object, T, object> handler)
        {
            master.messageHandlers.Add(topic, typeof(TM), handler);
            return this;
        }
        public MasterBuilder<T> WithMessageHandler<TM>(string topic, Action<object, T> noResponseHandler)
        {
            master.messageHandlers.Add(topic, typeof(TM), (o, master) => { noResponseHandler(o, master); return null; });
            return this;
        }
        public MasterBuilder<T> InLoop(bool inLoop = true)
        {
            runInLoop = inLoop;
            return this;
        }
        public async Task Run()
        {
            Build();
            master.communicator.Start();
            await Task.Run(() => { while (master.inLoop) master.Run(); });
        }
        void Build()
        {
            communicator.Master = master;
            master.messageHandlers.Service = master;

            communicator.serializer = serializer;
            master.messageHandlers.Serializer = serializer;

            master.communicator = communicator;

            master.inLoop = runInLoop;
        }
    }
}
