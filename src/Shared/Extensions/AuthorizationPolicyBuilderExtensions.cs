using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidBlazor.Shared.Extensions
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        private static readonly IEnumerable<string> _scopeClaimTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "http://schemas.microsoft.com/identity/claims/scope",
                "scope"
        };
        public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder builder, params string[] scopes)
        {
            return builder.RequireAssertion(context =>
                context.User
                    .Claims
                    .Where(c => _scopeClaimTypes.Contains(c.Type))
                    .SelectMany(c => c.Value.Split(' '))
                    .Any(s => scopes.Contains(s, StringComparer.OrdinalIgnoreCase))
            );
        }
    }
}
