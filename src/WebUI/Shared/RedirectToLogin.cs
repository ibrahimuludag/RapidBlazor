using Microsoft.AspNetCore.Components;
using System;

namespace RapidBlazor.WebUI.Shared
{
    public partial class RedirectToLogin
    {
        [Inject]
        public NavigationManager Navigation { get; set; }
        protected override void OnInitialized()
        {
            Navigation.NavigateTo($"authentication/login?returnUrl=" +
                $"{Uri.EscapeDataString(Navigation.Uri)}");
        }
    }
}
