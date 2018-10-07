using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PreparedLogger.DataAccess;
using PreparedLogger.DataAccess.SqlServer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PreparedLoggerContextExtensions_SqlServer
    {
        public static IServiceCollection AddPreparedLoggerContext_SqlServer(
            this IServiceCollection serviceCollection,
            string connectionString)
        {
            return serviceCollection.AddDbContext<PreparedLoggerContext, PreparedLoggerContext_SqlServer>(
                options => options.UseSqlServer(connectionString));
        }
    }
}