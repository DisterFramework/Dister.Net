using System.Collections.Generic;
using Dister.Net.Modules;
using Dister.Net.Service;

namespace Dister.Net.Variables.DiserVariables
{
    public abstract class DisterVariablesController<T> : Module<T>
    {
        internal abstract void SetDisterVariable(string name, object value);
        internal abstract Maybe<TV> GetDisterVariable<TV>(string name);
        internal abstract void Enqueue(string name, object value);
        internal abstract Maybe<TV> Dequeue<TV>(string name);
        internal abstract void AddQueue(string name);
        internal abstract void AddQueue(string name, object[] values);
        internal abstract void AddDictionary(string name);
        internal abstract void AddDictionary(string name, Dictionary<object, object> values);
        internal abstract Maybe<TV> GetFromDictionary<TV>(string name, object key);
        internal abstract void SetInDictionary(string name, object key, object value);
    }
}
