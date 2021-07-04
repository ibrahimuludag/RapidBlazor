using Microsoft.AspNetCore.Authorization;

namespace RapidBlazor.Shared.Policies
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
