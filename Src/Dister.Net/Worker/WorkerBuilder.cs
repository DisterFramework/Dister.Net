using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dister.Net.Communication.Worker;
using Dister.Net.Serialization;

namespace Dister.Net.Worker
{
    public class WorkerBuilder<T> where T : DisterWorker<T>
    {
        readonly T worker;
        WorkerCommunicator<T> communicator;
        ISerializer serializer;
        bool runInLoop = false;


        internal WorkerBuilder(T worker)
        {
            this.worker = worker ?? throw new ArgumentNullException(nameof(worker));
        }
        public WorkerBuilder<T> WithSerializer(ISerializer serializer)
        {
            this.serializer = serializer;
            return this;
        }
        public WorkerBuilder<T> WithCommunicator(WorkerCommunicator<T> communicator)
        {
            this.communicator = communicator;
            return this;
        }
        public WorkerBuilder<T> WithMessageHandler<TM>(string topic, Func<object, T, object> handler)
        {
            worker.messageHandlers.Add(topic, typeof(TM), handler);
            return this;
        }
        public WorkerBuilder<T> WithMessageHandler<TM>(string topic, Action<object, T> noResponseHandler)
        {
            worker.messageHandlers.Add(topic, typeof(TM), (o, master) => { noResponseHandler(o, master); return null; });
            return this;
        }
        public WorkerBuilder<T> InLoop(bool inLoop = true)
        {
            runInLoop = inLoop;
            return this;
        }
        public void Run()
        {
            Build();
            worker.communicator.Start();
            Task.Run(() => { while (worker.inLoop) worker.Run(); }).Wait();
        }
        void Build()
        {
            worker.serializer = serializer;
            worker.messageHandlers.Serializer = serializer;
            communicator.serializer = serializer;

            worker.messageHandlers.Service = worker;
            communicator.worker = worker;

            worker.communicator = communicator;

            worker.inLoop = runInLoop;
        }
    }
}
