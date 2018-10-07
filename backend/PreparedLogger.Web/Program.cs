﻿/*
To run the backend app, go to PreparedLogger.Web and run:

ASPNETCORE_ENVIRONMENT=Local dotnet watch run

You'll need to copy appSettings.Development.json to appSettings.Local.json and make
whatever config changes you want there first. To drop/recreate the DB with a new
schema (stop the watch first):

ASPNETCORE_ENVIRONMENT=Local dotnet ef database drop
ASPNETCORE_ENVIRONMENT=Local dotnet ef migrations remove --project ../PreparedLogger.DataAccess.SqlServer/
dotnet ef migrations add InitialCreate --project ../PreparedLogger.DataAccess.SqlServer/
ASPNETCORE_ENVIRONMENT=Local dotnet ef database update

Note that your local environment config will have to be configured for SQL Server. For
Sqlite, use the appropriate project and change the environment to Development.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PreparedLogger.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
