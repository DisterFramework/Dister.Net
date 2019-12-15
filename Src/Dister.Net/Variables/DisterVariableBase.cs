using System;

namespace Dister.Net.Variables
{
    public abstract class DisterVariableBase<TV, TS>
    {
        internal string name;
        internal DisterVariablesController<TS> disterVariablesController;

        public DisterVariableBase(string name, DisterVariablesController<TS> disterVariablesController)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.disterVariablesController = disterVariablesController ?? throw new ArgumentNullException(nameof(disterVariablesController));
        }
    }
}
