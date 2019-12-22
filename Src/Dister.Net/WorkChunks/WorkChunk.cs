using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Communication;
using Dister.Net.Communication.Message;
using Dister.Net.Service;

namespace Dister.Net.WorkChunks
{
    public class WorkChunk<T,TV>
    {
        internal string Id { get; set; }
        internal string Name { get; set; }
        internal DisterService<T> disterService { get; set; }
        public TV Value { get; set; }
        public void Response(object o)
        {
            var packet = new MessagePacket
            {
                Type = MessageType.WorkChunkResponse,
                Content = disterService.Serializer.Serialize(o),
                Topic = Name,
                Id = Id
            };
            disterService.Communicator.SendMessage(packet);
        }
    }
}
