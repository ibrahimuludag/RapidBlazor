using Microsoft.AspNetCore.Authorization;

namespace RapidBlazor.Shared.Security
{
    public static class Constants
    {
        public const string AdminRole = "admin";
        public static (string Name, AuthorizationPolicy Value)[] ApiPolicies => 
            new (string, AuthorizationPolicy)[] { 
                (nameof(Policies.AdminPolicy) , Policies.AdminPolicy),
                (nameof(Policies.PurgePolicy) , Policies.PurgePolicy)
        };

        public static (string Name, AuthorizationPolicy Value)[] WebUIPolicies =>
            new (string, AuthorizationPolicy)[] {
                (nameof(Policies.AdminPolicy) , Policies.AdminPolicy)
        };
    }
}
