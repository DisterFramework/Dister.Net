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

namespace Dister.Net.Communication.Master
{
    public class SockerMasterCommunicator<T> : Communicator<T>
    {
        readonly Socket listener;
        readonly ConcurrentBag<Socket> workerSockets = new ConcurrentBag<Socket>();
        readonly Thread acceptor;

        public SockerMasterCommunicator()
        {
            listener = CreateListenerSocket();
            acceptor = CreateAcceptor();
        }
        private Socket CreateListenerSocket()
        {
            var localhostIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last();
            var localEndpoint = new IPEndPoint(localhostIp, 1234);
            var s = new Socket(localhostIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(localEndpoint);
            return s;
        }
        private Thread CreateAcceptor()
        {
            return new Thread(() => AcceptConnections())
            {
                Name = "Acceptor"
            };
        }

        internal override void Start()
        {
            listener.Listen(2048);
            acceptor.Start();
        }

        private void AcceptConnections()
        {
            while (true)
            {
                Socket workerSocket = listener.Accept();
                workerSockets.Add(workerSocket);

                new Thread(() => ReceiveFromWorker(workerSocket))
                {
                    Name = "WorkerReceiver-" + Guid.NewGuid()
                }.Start();
            }
        }
        private void ReceiveFromWorker(Socket workerSocket)
        {
            while (workerSocket.IsOpen())
            {
                var message = workerSocket.ReceiveMessagePacket(service.Serializer);
                Task.Run(() => HandleMessage(message, workerSocket));
            }
            Console.WriteLine("Socket closed");//TODO: Change it later
        }
        private void HandleMessage(MessagePacket message, Socket workerSocket)
        {
            if (message.Type == MessageType.NoResponseRequest)
            {
                service.MessageHandlers.Handle(message);
            }
            else if(message.Type == MessageType.ResponseRequest)
            {
                var result = service.MessageHandlers.Handle(message);

                var response = new MessagePacket()
                {
                    Id = message.Id
                };

                if(result == null)
                {
                    response.Type = MessageType.NullResponse;
                }
                else
                {
                    response.Type = MessageType.Response;
                    response.Content = service.Serializer.Serialize(result);
                }

                var data = response.ToDataString(service.Serializer);
                Task.Run(() => workerSocket.SendAsync(data));
            }
        }

        internal override void SendMessage(MessagePacket messagePacket)
        {
            foreach (var socket in workerSockets)
            {
                Task.Run(() => socket.SendAsync(messagePacket.ToDataString(service.Serializer)));
            }
        }
        internal override TM GetResponse<TM>(MessagePacket messagePacket)
        {
            throw new NotImplementedException();
        }
        //internal override void SendMessage(string topic, object o)
        //{
        //    List<Task> listOfTasks = new List<Task>();

        //    foreach (var socket in workerSockets)
        //    {
        //        var content = new MessagePacket()
        //        {
        //            Type = MessageType.NoResponseRequest,
        //            Topic = topic,
        //            Content = service.serializer.Serialize(o)
        //        }.ToDataString(service.serializer);

        //        listOfTasks.Add(socket.SendAsync(content));
        //    }

        //    Task.WhenAll(listOfTasks);
        //}
    }
}
