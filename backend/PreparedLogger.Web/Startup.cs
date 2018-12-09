using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PreparedLogger.DataAccess;
using PreparedLogger.DataAccess.SqlServer;
using Swashbuckle.AspNetCore.Swagger;

namespace PreparedLogger.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("PreparedLogger");
            //System.Console.WriteLine("Connection String: " + connectionString);
            switch (Configuration.GetValue<string>("DbType"))
            {
                case "sqlite":
                    //System.Console.WriteLine("Using SQLite");
                    services.AddPreparedLoggerContext_Sqlite(connectionString);
                    break;
                case "sqlserver":
                    //System.Console.WriteLine("Using MS SQL Server");
                    services.AddPreparedLoggerContext_SqlServer(connectionString);
                    break;
                default:
                    throw new Exception("Invalid or missing application configuration; must specify a valid DbType");
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Prepared Logger API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // I think env.IsDevelopment() is stupid; you have to use MS's assumed environment names, which
            // I find restrictive (especially since I'm going to want to check appsettings.Development.json
            // into source control). So I don't use it; I made my own IsLocal config variable to do this.
            //if (env.IsDevelopment())
            bool isLocal = Configuration.GetValue<bool?>("IsLocal").GetValueOrDefault(false);
            //System.Console.WriteLine("IsLocal: " + isLocal);
            if (isLocal)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prepared Logger API v1");
            });
            app.UseMvc();
        }
    }
}
