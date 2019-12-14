using System;
using System.Collections.Concurrent;
using Dister.Net.Communication.Message;
using Dister.Net.Serialization;

namespace Dister.Net.Communication.Message
{
    internal class MessageHandlers<T>
    {
        private readonly ConcurrentDictionary<string, MessageHandler<T>> handlers = new ConcurrentDictionary<string, MessageHandler<T>>();
        //private readonly ConcurrentDictionary<string, Func<object, T, object>> handlers = new ConcurrentDictionary<string, Func<object, T, object>>();
        internal ISerializer Serializer { get; set; }
        internal T Service { get; set; }

        internal void Add(string topic, Type type, Func<object, T, object> handler)
        {
            var msgHandler = new MessageHandler<T>(type, handler);
            handlers.AddOrUpdate(topic, msgHandler, (x, y) => msgHandler);
        }
        internal object Handle(MessagePacket message)
        {
            if (handlers.ContainsKey(message.Topic))
                return handlers[message.Topic].Handle(Serializer, message.Content, Service);
            else
                throw new Exception($"No message handler for topic '{message.Topic}'");
        }
    }
    internal class MessageHandler<T>
    {
        Type type;
        Func<object, T, object> Handler { get; set; }

        public MessageHandler(Type type, Func<object, T, object> handler)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        internal object Handle(ISerializer serializer, string input, T service)
        {
            return Handler(serializer.Deserialize(input, type), service);
        }
    }
}
