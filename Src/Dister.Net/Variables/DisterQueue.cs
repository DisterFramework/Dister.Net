namespace Dister.Net.Variables
{
    public class DisterQueue<TV, TS> : DisterVariableBase<TV, TS>
    {
        public DisterQueue(string name, DisterVariablesController<TS> disterVariablesController) : base(name, disterVariablesController)
        {
        }

        public Maybe<TV> Dequeue() => disterVariablesController.Dequeue<TV>(name);
        public void Enqueue(TV value) => disterVariablesController.Enqueue(name, value);
    }
}
