using Microsoft.AspNetCore.Components;
using MudBlazor;
using RapidBlazor.WebUI.Api.Client;
using RapidBlazor.WebUI.Shared;
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
        
        [CascadingParameter]
        public MainLayout Layout { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.BreadcrumbItems = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("Personal", href: "#"),
                new BreadcrumbItem("Admin", href: "#"),
            };

            Message = await Client.GetAsync();
        }
    }
}
