using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Dister.Net.Communication.Message;
using Dister.Net.Exceptions.CommunicatorExceptions;
using Dister.Net.Helpers;
using Dister.Net.Variables;

namespace Dister.Net.Communication.SocketCommunicator
{
    public class MasterSocketCommunicator<T> : Communicator<T>
    {
        private readonly Socket listener;
        private readonly ConcurrentBag<Socket> workerSockets = new ConcurrentBag<Socket>();
        private readonly Thread acceptor;

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
                var workerSocket = listener.Accept();
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
            throw new ConnectionClosedException("Connecton to worker closed");
        }
        private void HandleMessage(MessagePacket messagePacket, Socket workerSocket)
        {
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
                var name = messagePacket.Topic;
                var value = service.Serializer.Deserialize<object>(messagePacket.Content);
                service.DisterVariablesController.SetDisterVariable(name, value);
            }
            else if (messagePacket.Type == MessageType.VariableGet)
            {
                var value = service.DisterVariablesController.GetDisterVariable<object>(messagePacket.Topic);
                MessagePacket response;
                if (value.IsSome)
                {
                    response = new MessagePacket
                    {
                        Id = messagePacket.Id,
                        Content = service.Serializer.Serialize(value.Value),
                        Type = MessageType.Response
                    };
                }
                else
                {
                    response = new MessagePacket
                    {
                        Id = messagePacket.Id,
                        Type = MessageType.NullResponse
                    };
                }

                workerSocket.Send(response.ToDataString(service.Serializer));
            }
            else if (messagePacket.Type == MessageType.Enqueue)
            {
                var name = messagePacket.Topic;
                var value = service.Serializer.Deserialize<object>(messagePacket.Content);
                service.DisterVariablesController.Enqueue(name, value);
            }
            else if (messagePacket.Type == MessageType.Dequeue)
            {
                var name = messagePacket.Topic;
                var value = service.DisterVariablesController.Dequeue<object>(name);
                MessagePacket response;
                if (value.IsSome)
                {
                    response = new MessagePacket
                    {
                        Id = messagePacket.Id,
                        Content = service.Serializer.Serialize(value.Value),
                        Type = MessageType.Response
                    };
                }
                else
                {
                    response = new MessagePacket
                    {
                        Id = messagePacket.Id,
                        Type = MessageType.NullResponse
                    };
                }

                workerSocket.Send(response.ToDataString(service.Serializer));
            }
            else if (messagePacket.Type == MessageType.DictionarySet)
            {
                var kvPair = service.Serializer.Deserialize<KeyValuePair<object, object>>(messagePacket.Content);
                var name = messagePacket.Topic;
                service.DisterVariablesController.SetInDictionary(name, kvPair.Key, kvPair.Value);
            }
            else if (messagePacket.Type == MessageType.DictionaryGet)
            {
                var key = service.Serializer.Deserialize<object>(messagePacket.Content);
                var name = messagePacket.Topic;
                var value = service.DisterVariablesController.GetFromDictionary<object>(name, key);

                MessagePacket response;
                if (value.IsSome)
                {
                    response = new MessagePacket
                    {
                        Id = messagePacket.Id,
                        Content = service.Serializer.Serialize(value.Value),
                        Type = MessageType.Response
                    };
                }
                else
                {
                    response = new MessagePacket
                    {
                        Id = messagePacket.Id,
                        Type = MessageType.NullResponse
                    };
                }

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
        internal override Maybe<TM> GetResponse<TM>(MessagePacket messagePacket) => throw new NotImplementedException();
    }
}
