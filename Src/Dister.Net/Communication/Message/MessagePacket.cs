using System;
using Dister.Net.Helpers;
using Dister.Net.Serialization;

namespace Dister.Net.Communication.Message
{
    /// <summary>
    /// Model used to transport data between <see cref="Service.DisterService{T}"/>
    /// </summary>
    internal class MessagePacket
    {
        public MessageType Type { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string Id { get; set; }

        /// <summary>
        /// Creates empty packet with random <see cref="MessagePacket.Id"/>
        /// </summary>
        public MessagePacket()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Creates ready to send string based on provided <see cref="ISerializer"/>
        /// </summary>
        /// <param name="serializer"><see cref="Service.DisterService{T}"/>'s <see cref="ISerializer"/></param>
        /// <returns>Ready to send string</returns>
        internal string ToDataString(ISerializer serializer)
        {
            var data = serializer.Serialize(this);
            var length = data.Length.ToHex();
            var content = length + data;
            return content;
        }
    }
}
