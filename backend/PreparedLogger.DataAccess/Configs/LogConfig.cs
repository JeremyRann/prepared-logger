using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PreparedLogger.Models;

namespace PreparedLogger.DataAccess.Configs
{
    public class LogConfig : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(255);
        }
    }
}