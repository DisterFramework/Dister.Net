using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Service;

namespace Dister.Net.Modules
{
    public abstract class Module<T>
    {
        internal DisterService<T> disterService;
        internal T service;
        internal virtual void Start() { }
    }
}
