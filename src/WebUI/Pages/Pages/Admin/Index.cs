using Microsoft.AspNetCore.Components;
using RapidBlazor.WebUI.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Pages.Pages.Admin
{
    public partial class Index
    {
        public string Message { get; set; }

        [Inject]
        private IAdminClient Client { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Message = await Client.GetAsync();
        }
    }
}
