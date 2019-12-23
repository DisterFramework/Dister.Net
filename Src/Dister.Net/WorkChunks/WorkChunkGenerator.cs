using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Dister.Net.Service;
using Dister.Net.Variables.DiserVariables;

namespace Dister.Net.WorkChunks
{
    internal class WorkChunkGenerator<T>
    {
        private readonly Func<T, Maybe<object>> generator;
        private readonly Action<object, T> responseHandler;
        private readonly Type responseType;
        private ConcurrentBag<KeyValuePair<string, object>> waitingChunks = new ConcurrentBag<KeyValuePair<string, object>>();
        public WorkChunkGenerator(Func<T, Maybe<object>> generator, Action<object, T> responseHandler, Type responseType)
        {
            this.generator = generator ?? throw new ArgumentNullException(nameof(generator));
            this.responseHandler = responseHandler ?? throw new ArgumentNullException(nameof(responseHandler));
            this.responseType = responseType;
        }

        internal Maybe<TV> Generate<TV>(T service, string id)
        {
            var result = generator(service);
            if (result.IsNone)
            {
                if (waitingChunks.Count == 0)
                    return Maybe<TV>.None();
                waitingChunks.TryPeek(out var value);
                return Maybe<TV>.Some((TV)value.Value);
            }
            waitingChunks.Add(new KeyValuePair<string, object>(id, result));
            return Maybe<TV>.Some((TV)result.Value);
        }
        internal void HandleResponse(string serialized, T service, DisterService<T> disterService, string id)
        {
            var o = disterService.Serializer.Deserialize(serialized, responseType);
            var arr = waitingChunks.ToArray();
            if (arr.Any(x => x.Key == id))
            {
                var currentChunk = arr.First(x => x.Key == id);
                waitingChunks = new ConcurrentBag<KeyValuePair<string, object>>(waitingChunks.Except(new[] { currentChunk }));
                responseHandler(o, service);
            }
        }
    }
}
