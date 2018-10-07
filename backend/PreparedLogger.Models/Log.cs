using System.Collections.Generic;

namespace PreparedLogger.Models
{
    public class Log
    {
        public int LogID { get; set; }
        public string Name { get; set; }

        public ICollection<LogEntry> LogEntries { get; set; }
    }
}