using System;

namespace Dister.Net.Exceptions.Handling
{
    internal class ExceptionHandler<T>
    {
        private Action<Exception, T> Handler { get; set; }

        public ExceptionHandler(Type type, Action<Exception, T> handler)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
        internal void Handle(Exception exception, T service)
            => Handler(exception, service);
    }
}
