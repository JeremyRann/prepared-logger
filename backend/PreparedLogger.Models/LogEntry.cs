using System;

namespace PreparedLogger.Models
{
    public class LogEntry
    {
        public int LogEntryID { get; set; }
        public int LogID { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }

        public Log Log { get; set; }
    }
}