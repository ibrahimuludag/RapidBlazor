using Microsoft.AspNetCore.Components;
using RapidBlazor.WebUI.Api.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Pages.Samples
{
    public partial class FetchData
    {
        protected IList<WeatherForecast> Forecasts { get; private set; }

        [Inject]
        private IWeatherForecastClient Client { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Forecasts = await Client.GetAsync();
        }
    }
}
