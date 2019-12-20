using System;

namespace Dister.Net.Logs.MasterStoredLogAggregator
{
    /// <summary>
    /// Master's client of MasterStoredLogAggregator
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Service.DisterService{T}"/></typeparam>
    public class MasterMasterStoredLogAggregator<T> : LogAggregator<T>
    {
        public override void Log(LogLevel logLevel, int eventId, object message) => Console.WriteLine($"[{logLevel} ({eventId})] {message}");
    }
}
