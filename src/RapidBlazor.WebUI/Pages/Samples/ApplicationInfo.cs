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
        public string AccessToken { get; set; }

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
    }
}
