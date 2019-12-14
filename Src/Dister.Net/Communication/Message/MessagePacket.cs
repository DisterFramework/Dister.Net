using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Helpers;
using Dister.Net.Serialization;

namespace Dister.Net.Communication.Message
{
    internal class MessagePacket
    {
        public MessagePacket()
        {
            Id = Guid.NewGuid().ToString();
        }

        public MessageType Type { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string Id { get; set; }

        internal string ToDataString(ISerializer serializer)
        {
            var data = serializer.Serialize(this);
            var length = data.Length.ToHex();
            var content = length + data;
            return content;
        }
    }
}
