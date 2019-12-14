using System.Collections.Generic;
using System.Threading.Tasks;
using Dister.Net.Communication.Master;
using Dister.Net.Communication.Message;

namespace Dister.Net.Master
{
    public abstract class DisterMaster<T> 
    {
        internal MasterCommunicator<T> communicator;
        internal MessageHandlers<T> messageHandlers = new MessageHandlers<T>();
        internal bool inLoop;
        public abstract void Run();
        public void SendToAllWorkers(string topic, object o)
            => Task.Run(() => communicator.SendToAllWorkers(topic, o));
        public void ExitLoop() => inLoop = false;
    }
}
