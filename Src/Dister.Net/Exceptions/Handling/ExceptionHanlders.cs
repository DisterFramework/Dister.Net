using System;
using System.Collections.Generic;

namespace Dister.Net.Exceptions.Handling
{
    internal class ExceptionHanlders<T>
    {
        private readonly Dictionary<Type, ExceptionHandler<T>> handlers = new Dictionary<Type, ExceptionHandler<T>>();
        internal T Service { get; set; }

        internal void Add(Type type, Action<Exception, T> handler)
        {
            var msgHandler = new ExceptionHandler<T>(type, handler);
            handlers.Add(type, msgHandler);
        }
        internal bool Handle(Exception exception)
        {
            if (handlers.ContainsKey(exception.GetType()))
            {
                handlers[exception.GetType()].Handle(exception, Service);
                return true;
            }
            return false;
        }
    }
}
