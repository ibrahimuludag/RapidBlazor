using RapidBlazor.Application;
using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Infrastructure;
using RapidBlazor.Infrastructure.Persistence;
using RapidBlazor.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Linq;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RapidBlazor.Api.Infra.Options;

namespace RapidBlazor.Api
{
    public class Startup
    {
        readonly string AllowedCorsOriginsPolicyName = "AllowedCorsOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddControllers()
                .AddFluentValidation();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "RapidBlazor API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            // https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html?highlight=apiscopes
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options => Configuration.Bind("JwtBearerOptions", options));

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("ApiScope", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.RequireClaim("scope", Configuration.GetValue<string>("ApplicationSettings:RequiredScope"));
                //});
                options.AddPolicy(nameof(Shared.Policies.ApiPolicy), Shared.Policies.ApiPolicy());

            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowedCorsOriginsPolicyName, builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("AllowedCorsOrigins").Get<string[]>())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            services.AddOptions<ApplicationSettings>()
                .Bind(Configuration.GetSection("ApplicationSettings"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint(new MigrationsEndPointOptions());
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });

            app.UseRouting();
            app.UseCors(AllowedCorsOriginsPolicyName);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                        .RequireAuthorization(nameof(Shared.Policies.ApiPolicy));
            });
        }
    }
}
