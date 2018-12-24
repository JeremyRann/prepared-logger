using Microsoft.EntityFrameworkCore;
using PreparedLogger.Data.Models;

namespace PreparedLogger.Data
{
    public class PreparedLoggerContext : DbContext
    {
        public PreparedLoggerContext(DbContextOptions<PreparedLoggerContext> options) : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
