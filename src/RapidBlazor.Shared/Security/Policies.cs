using Microsoft.AspNetCore.Authorization;

namespace RapidBlazor.Shared.Security
{
    public static class Policies
    {
        public static AuthorizationPolicy AdminPolicy = 
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("admin")
                .Build();
        
    }
}
