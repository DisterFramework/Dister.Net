using System.Threading.Tasks;
using Dister.Net.Master;
using Dister.Net.Serialization;

namespace Dister.Net.Communication.Master
{
    public abstract class MasterCommunicator<T>
    {
        internal ISerializer serializer;
        internal DisterMaster<T> Master { get; set; }
        internal abstract void Start();
        internal abstract void SendToAllWorkers(string topic, object o);
    }
}
