using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Pages.Samples
{
    public partial class UserClaims
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private ClaimsPrincipal User { get; set; }
        protected override async Task OnInitializedAsync()
        {
            AuthenticationState authenticationState;

            authenticationState = await authenticationStateTask;
            User = authenticationState.User;
            await base.OnInitializedAsync();

        }
    }

  
}
