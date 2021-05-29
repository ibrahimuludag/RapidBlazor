using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using RapidBlazor.WebUI.Models.Application;

namespace RapidBlazor.WebUI.Infra.Authentication
{
    public class RapidBlazorAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public RapidBlazorAuthorizationMessageHandler(IAccessTokenProvider accessTokenProvider,
            NavigationManager navigationManager, IOptions<ApplicationSettings> applicationSettings) : base(accessTokenProvider , navigationManager)
        {
            ConfigureHandler(authorizedUrls: new[] { applicationSettings.Value.ApiUrl });
        }
    }
}
