using Microsoft.AspNetCore.Components;
using RapidBlazor.WebUI.Api.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Pages.Monitor
{
    public class FetchDataBase : ComponentBase
    {
        protected IList<WeatherForecast> Forecasts { get; private set; }

        [Inject]
        private IWeatherForecastClient ApiClient { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Forecasts = await ApiClient.GetAsync();
        }
    }
}
