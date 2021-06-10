using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Infrastructure.Persistence;
using RapidBlazor.Infrastructure.Services;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidBlazor.DbMigration
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<IDomainEventService, Fakes.DomainEventService>();
                services.AddTransient<IDateTime, DateTimeService>();
                services.AddTransient<ICurrentUserService, Fakes.CurrentUserService>();
                services.AddTransient(c => hostContext.Configuration);

                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"), sql => sql.EnableRetryOnFailure()));
                services.AddHostedService<MigrateBackgroundService>();
            })
            .UseSerilog((hostingContext, loggerConfiguration) => {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
#if DEBUG
                // Used to filter out potentially bad data due debugging.
                // Very useful when doing Seq dashboards and want to remove logs under debugging session.
                loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
            });
    }
}
