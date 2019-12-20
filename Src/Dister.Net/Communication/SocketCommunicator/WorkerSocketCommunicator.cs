using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Dister.Net.Communication.Message;
using Dister.Net.Exceptions.CommunicatorExceptions;
using Dister.Net.Helpers;
using Dister.Net.Variables.DiserVariables;

namespace Dister.Net.Communication.SocketCommunicator
{
    /// <summary>
    /// Worker's client of SocketCommunicator
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Service.DisterService{T}"/></typeparam>
    public class WorkerSocketCommunicator<T> : Communicator<T>
    {
        private readonly string masterHostname;
        private Socket socket;
        private readonly ConcurrentDictionary<string, MessagePacket> responses = new ConcurrentDictionary<string, MessagePacket>();

        public WorkerSocketCommunicator(string masterHostname)
        {
            this.masterHostname = masterHostname ?? throw new ArgumentNullException(nameof(masterHostname));
        }
        private void ReceiveFromMaster()
        {
            while (socket.IsOpen())
            {
                var message = socket.ReceiveMessagePacket(disterService.Serializer);
                if (message.Type == MessageType.Response || message.Type == MessageType.NullResponse)
                {
                    responses.AddOrUpdate(message.Id, message, (x, y) => message);
                }
                else if (message.Type == MessageType.NoResponseRequest)
                {
                    disterService.MessageHandlers.Handle(message);
                }
            }
            throw new ConnectionClosedException("Connection to master closed");
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
            => socket.Send(messagePacket.ToDataString(disterService.Serializer));
        internal override Maybe<TM> GetResponse<TM>(MessagePacket messagePacket)
        {
            SendMessage(messagePacket);
            return GetFromResponses<TM>(messagePacket.Id);
        }
        private Maybe<TV> GetFromResponses<TV>(string key)
        {
            while (true)
            {
                if (responses.ContainsKey(key))
                {
                    responses.Remove(key, out var response);

                    if (response.Type == MessageType.NullResponse)
                        return Maybe<TV>.None();
                    else
                        return Maybe<TV>.Some(disterService.Serializer.Deserialize<TV>(response.Content));
                }
            }
        }
    }
}
