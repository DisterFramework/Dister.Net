using System.Collections.Generic;
using System.Text;
using Dister.Net.Communication.Message;

namespace Dister.Net.Logs.MasterStoredLogAggregator
{
    public class WorkerMasterStoredLogAggregator<T> : LogAggregator<T>
    {
        public override void Log(LogLevel logLevel, int eventId, object message)
        {
            var log = new Log(eventId, logLevel, service.Serializer.Serialize(message));
            var packet = new MessagePacket
            {
                Content = service.Serializer.Serialize(log),
                Type = MessageType.Log
            };
            service.Communicator.SendMessage(packet);
        }
    }
}
