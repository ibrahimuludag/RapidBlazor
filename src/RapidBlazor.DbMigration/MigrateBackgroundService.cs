using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Infrastructure.Persistence;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RapidBlazor.DbMigration
{
    public class MigrateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHost _host;
        private readonly ILogger<MigrateBackgroundService> _logger;

        public MigrateBackgroundService(ILogger<MigrateBackgroundService> logger, 
            IHost host, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _host = host;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Applying migrations to RapidBlazorDb");
                var dbContext = new ApplicationDbContextDesignTimeFactory().CreateDbContext(Array.Empty<string>());
                await dbContext.Database.MigrateAsync(stoppingToken);
                _logger.LogInformation("Finished migrations to RapidBlazorDb");

                await SeedData.EnsureSeeding(_serviceProvider);
            }
            catch (InvalidCredentialException ice)
            {
                _logger.LogError(ice, "Invalid credentials");
            }
            catch (Exception e)
            {
                _logger.LogError(e, MessageTemplate.Empty.ToString());
            }
            finally
            {
                await _host.StopAsync(stoppingToken);
            }
        }
    }
}
