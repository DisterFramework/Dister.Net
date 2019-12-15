using System;
using System.Collections.Generic;
using System.Text;
using Dister.Net.Communication;
using Dister.Net.Communication.Message;

namespace Dister.Net.Variables
{
    public class DisterVariable<TV, TS>
    {
        internal string name;
        internal DisterVariablesController<TS> disterVariablesController;

        public DisterVariable(string name, DisterVariablesController<TS> disterVariablesController)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.disterVariablesController = disterVariablesController ?? throw new ArgumentNullException(nameof(disterVariablesController));
        }

        public TV Get()
        {
            return disterVariablesController.GetDisterVariable<TV>(name);
        }
        public void Set(TV value)
        {
            disterVariablesController.SetDisterVariable(name,value);
        }
    }
}
