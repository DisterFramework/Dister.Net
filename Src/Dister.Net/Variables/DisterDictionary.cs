namespace Dister.Net.Variables
{
    public class DisterDictionary<TK, TV, TS> : DisterVariableBase<TV, TS>
    {
        public DisterDictionary(string name, DisterVariablesController<TS> disterVariablesController) : base(name, disterVariablesController)
        {
        }
        public Maybe<TV> this[TK key]
        {
            get => disterVariablesController.GetFromDictionary<TV>(name, key);
            set => disterVariablesController.SetInDictionary(name, key, value.Value);
        }
    }
}
