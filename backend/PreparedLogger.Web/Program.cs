/*
To run the backend app, you'll need to do three things:

1. Set the ASPNETCORE_ENVIRONMENT variable to the desired value. For local development,
it is recommended to use the value "Local". For bash:

$ export ASPNETCORE_ENVIRONMENT=local

For powershell:

> $Env:ASPNETCORE_ENVIRONMENT="Local"

2. Create a config file that matches your environment. It is recommended to copy
appsettings.Development.json to appsettings.Local.json (note it is case-sensitive) and
change any desired values. The most important setting here is the DB connection string.
Here's an example for a SQL Server connection:

"ConnectionStrings": {
  "PreparedLogger": "Server=srv;Database=db;User ID=myuser;Password=hunter2;"
},
"DbType": "sqlserver",

3. Configure the host. You'll want to create a new file called hostsettings.json that
looks like this:

{
  "server.urls": "http://localhost:5000"
}

Then you can run the app from your command line:

dotnet watch run

To drop/recreate the DB with a new schema (stop the watch first):

dotnet ef database drop
dotnet ef migrations remove --project ../PreparedLogger.DataAccess.SqlServer/
dotnet ef migrations add InitialCreate --project ../PreparedLogger.DataAccess.SqlServer/
dotnet ef database update

Swap where it says SqlServer for Sqlite for a Sqlite deployment.

For the frontend, I'm doing things like:

HOST=frontendip API_LOCATION="http://myip:5000" yarn run serve
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // The dotnet core team REALLY did not want you to specify the hostname in application settings,
            // so we'll add a locally .gitignore'd file to accomplish the same thing.
            // See https://andrewlock.net/configuring-urls-with-kestrel-iis-and-iis-express-with-asp-net-core/
            var hostConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hostingsettings.json", optional: true)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(hostConfig)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //System.Console.WriteLine("Environment Name: " + hostingContext.HostingEnvironment.EnvironmentName);
                })
                .UseStartup<Startup>();
        }
    }
}