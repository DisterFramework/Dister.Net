using System;
using System.Collections.Concurrent;
using Dister.Net.Communication.Message;
using Dister.Net.Exceptions.MessageHandlerExceptions;
using Dister.Net.Serialization;
using Dister.Net.Service;

namespace Dister.Net.Communication.Message
{
    internal class MessageHandlers<T>
    {
        private readonly ConcurrentDictionary<string, MessageHandler<T>> handlers = new ConcurrentDictionary<string, MessageHandler<T>>();
        internal T Service { get; set; }
        internal DisterService<T> DService { get; set; }

        internal void Add(string topic, Type type, Func<object, T, object> handler)
        {
            var msgHandler = new MessageHandler<T>(type, handler);
            handlers.AddOrUpdate(topic, msgHandler, (x, y) => msgHandler);
        }
        internal object Handle(MessagePacket message)
        {
            if (handlers.ContainsKey(message.Topic))
                return handlers[message.Topic].Handle(DService.Serializer, message.Content, Service);
            else
                throw new HandlerDoNotExistsException($"No existing handler for topic: '{message.Topic}'");
        }
    }
}
