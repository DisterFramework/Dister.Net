using System;
using Dister.Net.Serialization;

namespace Dister.Net.Communication.Message
{
    /// <summary>
    /// Holds message handler and type of message content object
    /// </summary>
    /// <typeparam name="T">Type of DisterService</typeparam>
    internal class MessageHandler<T>
    {
        /// <summary>
        /// <see cref="MessagePacket"/> content type
        /// </summary>
        private readonly Type type;
        /// <summary>
        /// Handler function
        /// </summary>
        private Func<object, T, object> Handler { get; set; }

        /// <summary>
        /// Creates new <see cref="MessageHandler{T}"/>
        /// </summary>
        /// <param name="type">Type of <see cref="MessagePacket"/> content objetc</param>
        /// <param name="handler">Handler function</param>
        public MessageHandler(Type type, Func<object, T, object> handler)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Run handler function
        /// </summary>
        /// <param name="serializer">Service's <see cref="ISerializer"/></param>
        /// <param name="input">Seerialized <see cref="MessagePacket"/> content</param>
        /// <param name="service">Type of <see cref="Dister.Net.Service.DisterService{T}"/></param>
        /// <returns>Output of handler</returns>
        internal object Handle(ISerializer serializer, string input, T service)
            => Handler(serializer.Deserialize(input, type), service);
    }
}
