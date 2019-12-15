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
        internal abstract TV GetDisterVariable<TV>(string name);
    }
}
