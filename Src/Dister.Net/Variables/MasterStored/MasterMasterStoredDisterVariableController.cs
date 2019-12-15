using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dister.Net.Exceptions.VariablesExceptions.DisterQueueExceptions;

namespace Dister.Net.Variables.MasterStored
{
    public class MasterMasterStoredDisterVariableController<T> : DisterVariablesController<T>
    {
        private readonly ConcurrentDictionary<string, object> variables = new ConcurrentDictionary<string, object>();
        private readonly ConcurrentDictionary<string, ConcurrentQueue<object>> queues = new ConcurrentDictionary<string, ConcurrentQueue<object>>();
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<object, object>> dictionaries = new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();

        internal override Maybe<TV> GetDisterVariable<TV>(string name)
        {
            if (variables.ContainsKey(name))
                return Maybe<TV>.Some((TV)variables[name]);
            else
                return Maybe<TV>.None();
        }
        internal override void SetDisterVariable(string name, object value) => variables.AddOrUpdate(name, value, (x, y) => value);

        internal override void AddQueue(string name)
            => queues.AddOrUpdate(name, new ConcurrentQueue<object>(), (x, y) => new ConcurrentQueue<object>());
        internal override void AddQueue(string name, object[] values)
            => queues.AddOrUpdate(name, new ConcurrentQueue<object>(values), (x, y) => new ConcurrentQueue<object>(values));
        internal override Maybe<TV> Dequeue<TV>(string name)
        {
            if (queues.ContainsKey(name))
            {
                if (queues[name].TryDequeue(out var result))
                    return Maybe<TV>.Some((TV)result);
                else
                    return Maybe<TV>.None();
            }
            else
            {
                throw new QueueNotExistException($"Queue '{name}' doesn't exist");
            }
        }
        internal override void Enqueue(string name, object value)
        {
            if (queues.ContainsKey(name))
            {
                queues[name].Enqueue(value);
            }
            else
            {
                throw new QueueNotExistException($"Queue '{name}' doesn't exist");
            }
        }

        internal override void AddDictionary(string name)
            => dictionaries.AddOrUpdate(name, new ConcurrentDictionary<object, object>(), (x, y) => new ConcurrentDictionary<object, object>());
        internal override void AddDictionary(string name, Dictionary<object, object> values)
            => dictionaries.AddOrUpdate(name, new ConcurrentDictionary<object, object>(values), (x, y) => new ConcurrentDictionary<object, object>(values));
        internal override Maybe<TV> GetFromDictionary<TV>(string name, object key)
        {
            if (!dictionaries.ContainsKey(name))
                throw new Exception();//TODO change this exception
            var dictionary = dictionaries[name];
            if (!dictionary.ContainsKey(key))
                return Maybe<TV>.None();
            return Maybe<TV>.Some((TV)dictionary[key]);
        }
        internal override void SetInDictionary(string name, object key, object value)
        {
            if (!dictionaries.ContainsKey(name))
                throw new Exception();//TODO change this exception
            var dictionary = dictionaries[name];
            dictionary[key] = value;
        }
    }
}
