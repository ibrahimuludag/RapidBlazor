using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MudBlazor.Services;
using RapidBlazor.WebUI.Models.Application;
using RapidBlazor.WebUI.Infra.Authentication;
using RapidBlazor.WebUI.Infra.Http;
using System.Linq;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace RapidBlazor.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
                builder.Configuration.Bind("UserOptions", options.UserOptions);
            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            ApplicationSettings appSettings = new ApplicationSettings();
            builder.Configuration.Bind("ApplicationSettings", appSettings);


            builder.Services.AddHttpClient<IRapidBlazorApiClient, RapidBlazorApiClient>(
                client => client.BaseAddress = new Uri(appSettings.ApiUrl))
            .AddHttpMessageHandler<RapidBlazorAuthorizationMessageHandler>()
            .AddPolicyHandler(Policies.GetRetryPolicy())
            .AddPolicyHandler(Policies.GetCircuitBreakerPolicy());

            builder.Services.AddMudServices();
            builder.Services.Configure<ApplicationSettings>(options =>
                builder.Configuration.Bind("ApplicationSettings", options) // TODO : Duplicate
            );

            builder.Services.AddTransient<RapidBlazorAuthorizationMessageHandler>();

            builder.Services.Scan(scan =>
                scan.FromCallingAssembly()
                .AddClasses(classes => classes.WithAttribute(typeof(System.CodeDom.Compiler.GeneratedCodeAttribute)))
                .AsMatchingInterface()
                .WithScopedLifetime());


            builder.Services.AddAuthorizationCore(options =>
            {
                foreach(var policy in RapidBlazor.Shared.Constants.WebUIPolicies)
                {
                    options.AddPolicy(policy.Name, policy.Value);
                }
            });

            await builder.Build().RunAsync();
        }
    }
}