using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Infrastructure.Persistence;
using RapidBlazor.Api;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using RapidBlazor.Application.IntegrationTests.Fakes;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private static Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
    private static DefaultHttpContext context = new();

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "RapidBlazor.Api"));

        services.AddLogging();

        startup.ConfigureServices(services);
        RegisterMockHttpContext(services);
        RegisterFakeAuthorizationService(services); // TODO : Make DefaultAuthorizationService to work with testing

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        };

        await EnsureDatabase();
    }

    private static void RegisterMockHttpContext(ServiceCollection services) {
        var currentDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
        services.Remove(currentDescriptor);
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        services.AddSingleton<IHttpContextAccessor>(mockHttpContextAccessor.Object);
    }

    private static void RegisterFakeAuthorizationService(ServiceCollection services)
    {
        var currentDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IAuthorizationService));
        services.Remove(currentDescriptor);
        services.AddSingleton<IAuthorizationService>(new FakeAuthorizationService());
    }
    private static async Task EnsureDatabase()
    {
        await RapidBlazor.DbMigration.Program.CreateHostBuilder(Array.Empty<string>()).RunConsoleAsync();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();       

        return await mediator.Send(request);
    }

    public static void RunAsAnonymUser()
    {
        var identity = new ClaimsIdentity(Array.Empty<Claim>());
        mockHttpContextAccessor.Object.HttpContext.User = new ClaimsPrincipal(identity);
    }

    public static string RunAsDefaultUser()
    {
        return RunAsUser("test@local", new string[] { });
    }

    public static string RunAsAdministrator()
    {
        return RunAsUser("administrator@local", new[] { "admin" });
    }

    public static string RunAsUser(string userName, string[] roles)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtClaimTypes.Name, userName),
            new Claim(JwtClaimTypes.GivenName, userName),
            new Claim(JwtClaimTypes.FamilyName, userName),
            new Claim(JwtClaimTypes.Email, $"{userName}@rapidblazor.com"),
            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
            new Claim(JwtClaimTypes.WebSite, "https://rapidblazor.com"),
            new Claim(JwtClaimTypes.Subject, userName),
            new Claim(JwtClaimTypes.PreferredUserName, userName)
        };
        
        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var identity = new ClaimsIdentity(claims);
        mockHttpContextAccessor.Object.HttpContext.User = new ClaimsPrincipal(identity);
        
        return userName;
    }

    public static async Task ResetState()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        RunAsAnonymUser();

    }

  
}
