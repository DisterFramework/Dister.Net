using Dister.Net.Master;
using Dister.Net.Worker;

namespace Dister.Net
{
    public class DisterBuilder
    {
        public WorkerBuilder<T> AsWorker<T>(T worker) where T : DisterWorker<T>
            => new WorkerBuilder<T>(worker);

        public MasterBuilder<T> AsMaster<T>(T master) where T : DisterMaster<T>
            => new MasterBuilder<T>(master);
    }
}
