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

namespace Dister.Net.Communication.SocketCommunicator
{
    public class MasterSocketCommunicator<T> : Communicator<T>
    {
        readonly Socket listener;
        readonly ConcurrentBag<Socket> workerSockets = new ConcurrentBag<Socket>();
        readonly Thread acceptor;

        public MasterSocketCommunicator()
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
                MessagePacket message;
                try
                {
                    message = workerSocket.ReceiveMessagePacket(service.Serializer);
                }
                catch (SocketException)
                {
                    break;
                }
                Task.Run(() => HandleMessage(message, workerSocket));
            }
            Console.WriteLine("Socket closed");//TODO: Change it later
        }
        private void HandleMessage(MessagePacket messagePacket, Socket workerSocket)
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine(messagePacket.Id);
            Console.WriteLine(messagePacket.Topic);
            Console.WriteLine(messagePacket.Content);
            Console.WriteLine(messagePacket.Type);

            if (messagePacket.Type == MessageType.NoResponseRequest)
            {
                service.MessageHandlers.Handle(messagePacket);
            }
            else if (messagePacket.Type == MessageType.ResponseRequest)
            {
                var result = service.MessageHandlers.Handle(messagePacket);

                var response = new MessagePacket()
                {
                    Id = messagePacket.Id
                };

                if (result == null)
                {
                    response.Type = MessageType.NullResponse;
                }
                else
                {
                    response.Type = MessageType.Response;
                    response.Content = service.Serializer.Serialize(result);
                }

                var data = response.ToDataString(service.Serializer);
                workerSocket.Send(data);
            }
            else if (messagePacket.Type == MessageType.VariableSet)
            {
                service.DisterVariablesController.SetDisterVariable(messagePacket.Topic, service.Serializer.Deserialize<object>(messagePacket.Content));
            }
            else if (messagePacket.Type == MessageType.VariableGet)
            {
                var value = service.DisterVariablesController.GetDisterVariable<object>(messagePacket.Topic);

                var response = new MessagePacket
                {
                    Id = messagePacket.Id,
                    Topic = messagePacket.Topic,
                    Content = service.Serializer.Serialize(value),
                    Type = MessageType.Response
                };
                workerSocket.Send(response.ToDataString(service.Serializer));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        internal override void SendMessage(MessagePacket messagePacket)
        {
            foreach (var socket in workerSockets)
            {
                socket.Send(messagePacket.ToDataString(service.Serializer));
            }
        }
        internal override TM GetResponse<TM>(MessagePacket messagePacket)
        {
            throw new NotImplementedException();
        }
    }
}
