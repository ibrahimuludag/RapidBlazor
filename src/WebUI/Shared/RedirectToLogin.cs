using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Shared
{
    public partial class RedirectToLogin
    {
        [Inject]
        public NavigationManager Navigation { get; set; }
        [CascadingParameter]
        Task<AuthenticationState> authenticationStateTask { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await authenticationStateTask;
            // User is logged in but does not have required policy
            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo($"authentication/login?returnUrl=" +
                $"{Uri.EscapeDataString(Navigation.Uri)}");
            }
            await base.OnInitializedAsync();
        }        
    }
}
