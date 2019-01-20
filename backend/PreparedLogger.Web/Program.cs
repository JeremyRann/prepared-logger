/* To start the API project, press F5 in VS Code or run "dotnet watch run" from a command line.
You can customize local settings with hostsettings.Local.json and appsettings.Local.json.
Example hostsettings.Local.json:

{
    "urls": "http://localhost:5000"
}

To create a database migration from scratch, remove existing migrations, navigate to
PreparedLogger.Data, and run the following:

dotnet ef migrations add InitialCreate --startup-project ../PreparedLogger.Web

After changing a model, create a migration with:

dotnet ef migrations add --startup-project ../PreparedLogger.Web

Running migrations is a tad easier; from PreparedLogger.Web:

dotnet ef database update

And of course  you can remove with:

dotnet ef database drop

To run the frontend:
HOST=frontendip API_LOCATION="https://myip:5001" yarn run serve */

using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PreparedLogger.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        // TODO: Consider merging auth and web projects into one using something like this:
        // https://www.strathweb.com/2017/04/running-multiple-independent-asp-net-core-pipelines-side-by-side-in-the-same-application/
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
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
                .UseStartup<Startup>();
    }
}
