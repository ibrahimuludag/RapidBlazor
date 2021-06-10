using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RapidBlazor.Infrastructure.Persistence;
using RapidBlazor.Infrastructure.Services;
using System.Reflection;

namespace RapidBlazor.DbMigration
{
    public class ApplicationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", true, true)
             .AddEnvironmentVariables()
             .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseSqlServer(connection, options => options.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name).EnableRetryOnFailure());

            return new ApplicationDbContext(optionsBuilder.Options, new Fakes.CurrentUserService(), new Fakes.DomainEventService(),  new DateTimeService());
        }
    }
}
