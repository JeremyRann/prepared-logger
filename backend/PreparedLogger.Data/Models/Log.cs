using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PreparedLogger.Data.Models
{
    public class Log
    {
        public int LogID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<LogEntry> LogEntries { get; set; }
    }

}