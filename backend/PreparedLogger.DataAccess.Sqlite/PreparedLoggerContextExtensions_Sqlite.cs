using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PreparedLogger.DataAccess;
using PreparedLogger.DataAccess.Sqlite;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PreparedLoggerContextExtensions_Sqlite
    {
        public static IServiceCollection AddPreparedLoggerContext_Sqlite(
            this IServiceCollection serviceCollection,
            string connectionString)
        {
            return serviceCollection.AddDbContext<PreparedLoggerContext, PreparedLoggerContext_Sqlite>(
                options => options.UseSqlite(connectionString));
        }
    }
}
