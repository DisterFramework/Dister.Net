namespace Dister.Net.Variables
{
    public class DisterVariable<TV, TS> : DisterVariableBase<TV, TS>
    {
        public DisterVariable(string name, DisterVariablesController<TS> disterVariablesController) : base(name, disterVariablesController)
        {
        }

        public Maybe<TV> Get() => disterVariablesController.GetDisterVariable<TV>(name);
        public void Set(TV value) => disterVariablesController.SetDisterVariable(name, value);
    }
}
