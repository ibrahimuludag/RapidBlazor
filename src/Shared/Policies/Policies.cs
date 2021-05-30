using Microsoft.AspNetCore.Authorization;

namespace RapidBlazor.Shared
{
    public static class Policies
    {
        public static AuthorizationPolicy ApiPolicy = 
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "rapidblazor.api.scope") 
                .Build();

        public static AuthorizationPolicy AdminPolicy = 
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("admin")
                .Build();
        
    }
}
