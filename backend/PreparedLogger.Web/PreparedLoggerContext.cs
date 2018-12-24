using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PreparedLogger.Web
{
    public class PreparedLoggerContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=PreparedLogger_Local;User Id=sa;Password=ImCOEWLZpiPV303N;");
        }
    }

    public class Log
    {
        public int LogID { get; set; }
        public string Name { get; set; }

        public ICollection<LogEntry> LogEntries { get; set; }
    }

    public class LogEntry
    {
        public int LogEntryID { get; set; }
        public int LogID { get; set; }
        public string Text { get; set; }
    }
}