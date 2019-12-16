using System;

namespace Dister.Net.Logs.MasterStoredLogAggregator
{
    public class MasterMasterStoredLogAggregator<T> : LogAggregator<T>
    {
        public override void Log(LogLevel logLevel, int eventId, object message) => Console.WriteLine($"[{logLevel} ({eventId})] {message}");
    }
}
