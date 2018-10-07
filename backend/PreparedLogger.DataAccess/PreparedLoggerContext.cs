using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PreparedLogger.DataAccess.Configs;
using PreparedLogger.Models;

namespace PreparedLogger.DataAccess
{
    public abstract class PreparedLoggerContext : DbContext
    {
        public PreparedLoggerContext(DbContextOptions options)
            : base(options)
        {
        }

        protected void OnModelCreating_Base(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogConfig());
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
