using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Dister.Net.Communication.Message;
using Dister.Net.Serialization;

namespace Dister.Net.Helpers
{
    internal static class SocketHelpers
    {
        internal static Task Send(this Socket socket, string data) 
            => Task.Run(() => socket.Send(Encoding.UTF8.GetBytes(data)));
        internal static bool IsOpen(this Socket s)
            => !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        /// <summary>
        /// Receive n bytes from <see cref="Socket"/>
        /// </summary>
        /// <param name="socket"><see cref="Socket"/> to read from</param>
        /// <param name="n">Bytes count to receive</param>
        /// <returns>Received bytes as <see cref="string"/></returns>
        internal static string Receive(this Socket socket, int n)
        {
            var buffer = new byte[n];
            socket.Receive(buffer, 0, n, SocketFlags.None);
            return Encoding.UTF8.GetString(buffer);
        }
        /// <summary>
        /// Receive <see cref="MessagePacket"/> from <see cref="Socket"/>
        /// </summary>
        /// <param name="socket"><see cref="Socket"/> to receive from</param>
        /// <param name="serializer"><see cref="ISerializer"/> used to deserialize <see cref="MessagePacket"/></param>
        /// <returns><see cref="MessagePacket"/> object</returns>
        internal static MessagePacket ReceiveMessagePacket(this Socket socket, ISerializer serializer)
        {
            var length = socket.Receive(8).FromHex();
            var data = socket.Receive(length);
            var message = serializer.Deserialize<MessagePacket>(data);
            return message;
        }
    }
}
