using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Communication.Message;
using Dister.Net.Service;
using Dister.Net.Variables;

namespace Dister.Net.Communication
{
    public abstract class Communicator<T>
    {
        internal DisterService<T> service;
        internal abstract void Start();
        internal abstract void SendMessage(MessagePacket messagePacket);
        internal abstract Maybe<TM> GetResponse<TM>(MessagePacket messagePacket);
    }
}
