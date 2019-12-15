using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Variables.MasterStored
{
    public class MasterMasterStoredDisterVariableController<T> : DisterVariablesController<T>
    {
        private ConcurrentDictionary<string, object> variables = new ConcurrentDictionary<string, object>();

        internal override TV GetDisterVariable<TV>(string name)
        {
            if (variables.ContainsKey(name))
                return (TV)variables[name];
            else
                throw new KeyNotFoundException();
        }
        internal override void SetDisterVariable(string name, object value)
        {
            variables.AddOrUpdate(name, value, (x, y) => value);
        }
    }
}
