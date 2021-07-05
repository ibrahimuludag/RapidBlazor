using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Infrastructure.Files;
using RapidBlazor.Infrastructure.Persistence;
using RapidBlazor.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace RapidBlazor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("RapidBlazorDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions => {
                            sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                            sqlOptions.EnableRetryOnFailure( // TODO : Make this configuration
                               maxRetryCount: 10,
                               maxRetryDelay: TimeSpan.FromSeconds(30),
                               errorNumbersToAdd: null);
                        }));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            // https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html?highlight=apiscopes
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options => configuration.Bind("JwtBearerOptions", options));

            services.AddAuthorization(options =>
            {
                foreach (var policy in Shared.Security.Constants.ApiPolicies)
                {
                    options.AddPolicy(policy.Name, policy.Value);
                }
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: Constants.AllowedCorsOriginsPolicyName, builder =>
                {
                    builder.WithOrigins(configuration.GetSection("AllowedCorsOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            return services;
        }
    }
}