using System;
using Dister.Net.Serialization;

namespace Dister.Net.Communication.Message
{
    internal class MessageHandler<T>
    {
        private readonly Type type;
        private Func<object, T, object> Handler { get; set; }

        public MessageHandler(Type type, Func<object, T, object> handler)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        internal object Handle(ISerializer serializer, string input, T service) 
            => Handler(serializer.Deserialize(input, type), service);
    }
}
