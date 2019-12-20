using System;
using System.Collections.Concurrent;
using Dister.Net.Exceptions.MessageHandlerExceptions;
using Dister.Net.Modules;
using Dister.Net.Service;

namespace Dister.Net.Communication.Message
{
    /// <summary>
    /// Contains message handlers and service info
    /// </summary>
    /// <typeparam name="T">Type of <see cref="DisterService{T}"/></typeparam>
    internal class MessageHandlers<T> : Module<T>
    {
        /// <summary>
        /// Dictionary of message handlers
        /// </summary>
        private readonly ConcurrentDictionary<string, MessageHandler<T>> handlers = new ConcurrentDictionary<string, MessageHandler<T>>();

        /// <summary>
        /// Adds new message handler
        /// </summary>
        /// <param name="topic">Topic of <see cref="MessagePacket"/></param>
        /// <param name="type">Type of <see cref="MessagePacket"/> content</param>
        /// <param name="handler">Handler function</param>
        internal void Add(string topic, Type type, Func<object, T, object> handler)
        {
            var msgHandler = new MessageHandler<T>(type, handler);
            handlers.AddOrUpdate(topic, msgHandler, (x, y) => msgHandler);
        }
        /// <summary>
        /// Handles <see cref="MessagePacket"/>
        /// </summary>
        /// <param name="message"><see cref="MessagePacket"/> to handle</param>
        /// <returns></returns>
        internal object Handle(MessagePacket message)
        {
            if (handlers.ContainsKey(message.Topic))
                return handlers[message.Topic].Handle(disterService.Serializer, message.Content, service);
            else
                throw new HandlerDoNotExistsException($"No existing handler for topic: '{message.Topic}'");
        }
    }
}
