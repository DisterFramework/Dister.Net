using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dister.Net.Serialization;
using Dister.Net.Worker;

namespace Dister.Net.Communication.Worker
{
    public abstract class WorkerCommunicator<TW>
    {
        internal DisterWorker<TW> worker;
        internal ISerializer serializer;

        internal abstract void SendMessageWithoutResponseRequestAsync(string topic, object o);
        internal abstract T SendMessageWithResponseRequestAsync<T>(string topic, object o) where T : class;
        internal abstract void Start();
    }
}
