using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Service;

namespace Dister.Net.Variables
{
    public abstract class DisterVariablesController<T>
    {
        internal DisterService<T> service;
        internal abstract void SetDisterVariable(string name, object value);
        internal abstract Maybe<TV> GetDisterVariable<TV>(string name);
        internal abstract void Enqueue(string name, object value);
        internal abstract Maybe<TV> Dequeue<TV>(string name);
        internal abstract void AddQueue(string name);
        internal abstract void AddQueue(string name, object[] values);
    }
}
