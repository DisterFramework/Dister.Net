﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Dister.Net.Variables.MasterStored
{
    public class MasterMasterStoredDisterVariableController<T> : DisterVariablesController<T>
    {
        private readonly ConcurrentDictionary<string, object> variables = new ConcurrentDictionary<string, object>();
        private readonly ConcurrentDictionary<string, ConcurrentQueue<object>> queues = new ConcurrentDictionary<string, ConcurrentQueue<object>>();

        internal override void AddQueue(string name) => queues.AddOrUpdate(name, new ConcurrentQueue<object>(), (x, y) => new ConcurrentQueue<object>());
        internal override void AddQueue(string name, object[] values)
        {
            AddQueue(name);
            foreach (var value in values)
            {
                Enqueue(name, value);
            }
        }

        internal override TV Dequeue<TV>(string name)
        {
            if (queues.ContainsKey(name))
            {
                if (queues[name].TryDequeue(out var result))
                    return (TV)result;
                else
                    throw new Exception();
            }
            else
            {
                throw new KeyNotFoundException();
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
                throw new KeyNotFoundException();
            }
        }

        internal override TV GetDisterVariable<TV>(string name)
        {
            if (variables.ContainsKey(name))
                return (TV)variables[name];
            else
                throw new KeyNotFoundException();
        }
        internal override void SetDisterVariable(string name, object value) => variables.AddOrUpdate(name, value, (x, y) => value);
    }
}