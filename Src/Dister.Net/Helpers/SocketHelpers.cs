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
        internal static Task SendAsync(this Socket socket, string data) 
            => Task.Run(() => socket.Send(Encoding.UTF8.GetBytes(data)));
        internal static void Send(this Socket socket, string data) 
            => socket.Send(Encoding.UTF8.GetBytes(data));
        internal static bool IsOpen(this Socket s)
            => !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);

        internal static string Receive(this Socket socket, int n)
        {
            byte[] buffer = new byte[n];
            socket.Receive(buffer, 0, n, SocketFlags.None);
            return Encoding.UTF8.GetString(buffer);
        }

        internal static MessagePacket ReceiveMessagePacket(this Socket socket, ISerializer serializer)
        {
            var length = socket.Receive(8).FromHex();
            var data = socket.Receive(length);
            var message = serializer.Deserialize<MessagePacket>(data);
            return message;
        }
    }
}
