using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dister.Net.Communication.Message;
using Dister.Net.Helpers;
using Dister.Net.Serialization;

namespace Dister.Net.Communication.Worker
{
    public class SocketWorkerCommunicator<T> : Communicator<T>
    {
        readonly string masterHostname;
        Socket socket;
        readonly ConcurrentDictionary<string, MessagePacket> responses = new ConcurrentDictionary<string, MessagePacket>();

        public SocketWorkerCommunicator(string masterHostname)
        {
            this.masterHostname = masterHostname ?? throw new ArgumentNullException(nameof(masterHostname));
        }

        //internal override void SendMessageWithoutResponseRequestAsync(string topic, object o)
        //{
        //    var content = new MessagePacket()
        //    {
        //        Type = MessageType.NoResponseRequest,
        //        Topic = topic,
        //        Content = serializer.Serialize(o)
        //    }.ToDataString(serializer);

        //    Task.Run(() => socket.SendAsync(content));
        //}
        //internal override T SendMessageWithResponseRequestAsync<T>(string topic, object o) 
        //{
        //    var message = new MessagePacket()
        //    {
        //        Type = MessageType.ResponseRequest,
        //        Topic = topic,
        //        Content = serializer.Serialize(o)
        //    };
        //    var id = message.Id;

        //    Task.Run(() => socket.SendAsync(message.ToDataString(serializer)));

        //    while (true)
        //    {
        //        if (responses.ContainsKey(id))
        //        {
        //            responses.Remove(id, out var response);

        //            if (response.Type == MessageType.NullResponse)
        //                return null;
        //            else 
        //                return serializer.Deserialize<T>(response.Content);
        //        }
        //    }
        //}
        private void ReceiveFromMaster()
        {
            while (socket.IsOpen())
            {
                var message = socket.ReceiveMessagePacket(service.Serializer);
                if (message.Type == MessageType.Response || message.Type == MessageType.NullResponse)
                {
                    responses.AddOrUpdate(message.Id, message, (x, y) => message);
                }
                else if (message.Type == MessageType.NoResponseRequest)
                {
                    service.MessageHandlers.Handle(message);
                }
            }
            Console.WriteLine("Socket closed");//TODO change it
        }
        internal override void Start()
        {
            var hostnames = Dns.GetHostEntry(masterHostname).AddressList;
            var masterIp = hostnames.Last();
            var localEndpoint = new IPEndPoint(masterIp, 1234);
            socket = new Socket(masterIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(localEndpoint);

            new Thread(() => ReceiveFromMaster())
            {
                Name = "Receiver"
            }.Start();
        }

        internal override void SendMessage(MessagePacket messagePacket)
        {
            Task.Run(() => socket.SendAsync(messagePacket.ToDataString(service.Serializer)));
        }

        internal override TM GetResponse<TM>(MessagePacket messagePacket)
        {
            SendMessage(messagePacket);

            while (true)
            {
                if (responses.ContainsKey(messagePacket.Id))
                {
                    responses.Remove(messagePacket.Id, out var response);

                    if (response.Type == MessageType.NullResponse)
                        return null;
                    else
                        return service.Serializer.Deserialize<TM>(response.Content);
                }
            }
        }
    }
}
