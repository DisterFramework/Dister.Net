using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dister.Net.Communication.Message;
using Dister.Net.Communication.Worker;
using Dister.Net.Serialization;

namespace Dister.Net.Worker
{
    public abstract class DisterWorker<T> 
    {
        internal WorkerCommunicator<T> communicator;
        internal MessageHandlers<T> messageHandlers = new MessageHandlers<T>();
        internal ISerializer serializer;
        internal bool inLoop;

        public string Name { get; set; } = "Worker-" + Guid.NewGuid();

        public void SendMessage(string topic, object o)
        {
            Task.Run(() => communicator.SendMessageWithoutResponseRequestAsync(topic, o));
        }
        public async Task<TM> GetResponse<TM>(string topic, object o) where TM : class
        {
            var result = await Task.Run(() => communicator.SendMessageWithResponseRequestAsync<TM>(topic, o));
            return result;
        }
        public abstract void Run();
        public void ExitLoop() => inLoop = false;
    }
}
