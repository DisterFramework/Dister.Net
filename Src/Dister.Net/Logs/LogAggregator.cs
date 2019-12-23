using Dister.Net.Modules;
using Dister.Net.Service;

namespace Dister.Net.Logs
{
    /// <summary>
    /// Base LogAggregator
    /// </summary>
    /// <typeparam name="T">Type of <see cref="DisterService{T}"/></typeparam>
    public abstract class LogAggregator<T> : Module<T>
    {
        public abstract void Log(LogLevel logLevel, int eventId, object message);
        public void Log(Log log) => Log(log.LogLevel, log.EventId, log.Message);
        public void LogDebug(int eventId, object message) => Log(LogLevel.Debug, eventId, message);
        public void LogInfo(int eventId, object message) => Log(LogLevel.Info, eventId, message);
        public void LogWarning(int eventId, object message) => Log(LogLevel.Warning, eventId, message);
        public void LogError(int eventId, object message) => Log(LogLevel.Error, eventId, message);
        public void LogCritical(int eventId, object message) => Log(LogLevel.Critical, eventId, message);
    }
}
