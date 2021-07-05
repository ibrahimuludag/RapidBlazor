using Microsoft.AspNetCore.Authorization;

namespace RapidBlazor.Shared.Security
{
    public static class Policies
    {
        public static AuthorizationPolicy AdminPolicy = 
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Constants.AdminRole)
                .Build();

        public static AuthorizationPolicy PurgePolicy =
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Constants.AdminRole)
                .Build();

    }
}
