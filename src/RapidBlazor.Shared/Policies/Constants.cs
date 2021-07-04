using Microsoft.AspNetCore.Authorization;

namespace RapidBlazor.Shared.Policies
{
    public static class Constants
    {
        public static (string Name, AuthorizationPolicy Value)[] ApiPolicies => 
            new (string, AuthorizationPolicy)[] { 
                (nameof(Policies.AdminPolicy) , Policies.AdminPolicy)
        };

        public static (string Name, AuthorizationPolicy Value)[] WebUIPolicies =>
            new (string, AuthorizationPolicy)[] {
                (nameof(Policies.AdminPolicy) , Policies.AdminPolicy)
        };
    }
}
