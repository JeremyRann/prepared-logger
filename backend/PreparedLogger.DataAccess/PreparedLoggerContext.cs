using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PreparedLogger.DataAccess.Configs;
using PreparedLogger.Models;

namespace PreparedLogger.DataAccess
{
    public class PreparedLoggerContext : DbContext
    {
        public PreparedLoggerContext(DbContextOptions<PreparedLoggerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogConfig());

            // https://github.com/aspnet/Announcements/issues/167#issue-146112393
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
