using Dister.Net.Communication.Message;

namespace Dister.Net.Logs.MasterStoredLogAggregator
{
    /// <summary>
    /// Workers's client of MasterStoredLogAggregator
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Service.DisterService{T}"/></typeparam>
    public class WorkerMasterStoredLogAggregator<T> : LogAggregator<T>
    {
        public override void Log(LogLevel logLevel, int eventId, object message)
        {
            var log = new Log(eventId, logLevel, disterService.Serializer.Serialize(message));
            var packet = new MessagePacket
            {
                Content = disterService.Serializer.Serialize(log),
                Type = MessageType.Log
            };
            disterService.Communicator.SendMessage(packet);
        }
    }
}
