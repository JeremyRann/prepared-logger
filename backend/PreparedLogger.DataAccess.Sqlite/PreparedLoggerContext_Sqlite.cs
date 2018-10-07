using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PreparedLogger.DataAccess.Sqlite
{
    public class PreparedLoggerContext_Sqlite : PreparedLoggerContext
    {
        public PreparedLoggerContext_Sqlite(DbContextOptions<PreparedLoggerContext_Sqlite> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating_Base(modelBuilder);
            // https://github.com/aspnet/Announcements/issues/167#issue-146112393
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }
    }
}
