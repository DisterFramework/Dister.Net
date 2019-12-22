using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dister.Net.Modules;
using Dister.Net.Variables.DiserVariables;

namespace Dister.Net.WorkChunks
{
    internal class WorkChunkGenerators<T> : Module<T>
    {
        private readonly Dictionary<string, WorkChunkGenerator<T>> generators = new Dictionary<string, WorkChunkGenerator<T>>();
        internal void Add(string name, Func<T, Maybe<object>> generator, Action<object, T> responseHandler, Type responseType)
            => generators.Add(name, new WorkChunkGenerator<T>(generator, responseHandler, responseType));
        internal Maybe<TV> Generete<TV>(string name, string id)
            => generators[name].Generate<TV>(service, id);
        internal void HandleResponse(string name, string id, string serialized)
            => Task.Run(() => generators[name].HandleResponse(serialized, service, disterService, id));
    }
}
