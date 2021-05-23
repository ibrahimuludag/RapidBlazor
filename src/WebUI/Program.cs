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
            });

            ApplicationSettings appSettings = new ApplicationSettings();
            builder.Configuration.Bind("AppSettings", appSettings);

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(appSettings.ApiUrl)});

            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}