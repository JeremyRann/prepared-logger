// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

/* EF Migrations:

dotnet ef migrations add ApplicationInitialCreate -c ApplicationDbContext -o Data/Migrations/IdentityServer/ApplicationDb
dotnet ef migrations add PersistedGrantInitialCreate -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add ConfigurationInitialCreate -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb

(always run with -c (context))
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run -- /seed
*/

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;
using System.Linq;

namespace PreparedLogger.Auth.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var seed = args.Any(x => x == "/seed");
            if (seed) args = args.Except(new[] { "/seed" }).ToArray();

            var host = CreateWebHostBuilder(args).Build();

            if (seed)
            {
                var config = host.Services.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("default");
                SeedData.EnsureSeedData(connectionString);
                return;
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddCommandLine(args)
                    .AddJsonFile("hostsettings.Local.json", optional: true)
                    .Build()
                )
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.Local.json", optional: true);
                })
                .UseStartup<Startup>()
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"identityserver4_log.txt")
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                });
        }
    }
}
