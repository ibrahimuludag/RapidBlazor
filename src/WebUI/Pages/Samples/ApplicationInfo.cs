using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Pages.Samples
{
    public partial class ApplicationInfo
    {
        [Inject]
        public IAccessTokenProvider AccessTokenProvider { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        public string AccessToken { get; set; }
        private string IsAdmin { get; set; }


        protected override async Task OnInitializedAsync()
        {
            // Move this to a service
            var tokenResult = await AccessTokenProvider.RequestAccessToken();

            if (tokenResult.TryGetToken(out var token))
            {
                AccessToken = token.Value;
            }
            
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            AuthenticationState authenticationState;

            authenticationState = await authenticationStateTask;
            //IsAdmin = authenticationState.User.IsInRole("admin");
            IsAdmin = string.Join( "<br />", authenticationState.User.Claims.ToArray().Select(c => c.Type + " " + c.Value));

        }
    }
}
