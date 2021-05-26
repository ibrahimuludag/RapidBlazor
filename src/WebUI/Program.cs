using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Services;
using RapidBlazor.WebUI.Models.Application;
using RapidBlazor.WebUI.Infra.Authentication;
using RapidBlazor.WebUI.Infra.HttpClients;
using RapidBlazor.WebUI.Api.Client;

namespace RapidBlazor.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOidcAuthentication(options =>
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions)
            );

            ApplicationSettings appSettings = new ApplicationSettings();
            builder.Configuration.Bind("ApplicationSettings", appSettings);


            builder.Services.AddHttpClient<IRapidBlazorApiClient, RapidBlazorApiClient>(
               client => client.BaseAddress = new Uri(appSettings.ApiUrl))
               .AddHttpMessageHandler<RapidBlazorAuthorizationMessageHandler>();

            builder.Services.AddMudServices();
            builder.Services.Configure<ApplicationSettings>(options =>
                builder.Configuration.Bind("ApplicationSettings", options)
            );

            builder.Services.AddTransient<RapidBlazorAuthorizationMessageHandler>();
            builder.Services.AddScoped<IWeatherForecastClient, WeatherForecastClient>();


            await builder.Build().RunAsync();
        }
    }
}