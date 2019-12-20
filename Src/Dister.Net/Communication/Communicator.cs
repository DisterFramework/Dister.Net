using Dister.Net.Communication.Message;
using Dister.Net.Modules;
using Dister.Net.Service;
using Dister.Net.Variables.DiserVariables;

namespace Dister.Net.Communication
{
    /// <summary>
    /// Base communicator class
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Service.DisterService{T}"/></typeparam>
    public abstract class Communicator<T> : Module<T>
    {
        internal abstract void SendMessage(MessagePacket messagePacket);
        internal abstract Maybe<TM> GetResponse<TM>(MessagePacket messagePacket);
    }
}
