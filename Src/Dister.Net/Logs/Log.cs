using System;

namespace Dister.Net.Logs
{
    public class Log
    {
        public int EventId { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }

        public Log(int eventId, LogLevel logLevel, string message)
        {
            EventId = eventId;
            LogLevel = logLevel;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public Log()
        {
        }
    }
}
