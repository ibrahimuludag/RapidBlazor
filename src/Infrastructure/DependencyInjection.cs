using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Infrastructure.Files;
using RapidBlazor.Infrastructure.Persistence;
using RapidBlazor.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidBlazor.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
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
                // TODO : 
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));

                //options.AddPolicy("ApiScope", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.RequireClaim("scope", Configuration.GetValue<string>("ApplicationSettings:RequiredScope"));
                //});
                options.AddPolicy(nameof(Shared.Policies.ApiPolicy), Shared.Policies.ApiPolicy());

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

            services.AddOptions<ApplicationSettings>()
                .Bind(configuration.GetSection("ApplicationSettings"));

            return services;
        }
    }
}