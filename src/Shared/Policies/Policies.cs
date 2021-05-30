using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidBlazor.Shared
{
    public static class Policies
    {
        // TODO : RequireClaim does not work with array values in Blazor. Be careful with RequireClaim in WASM
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
