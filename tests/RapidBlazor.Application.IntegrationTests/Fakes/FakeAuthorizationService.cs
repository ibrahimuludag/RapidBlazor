using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RapidBlazor.Application.IntegrationTests.Fakes
{
    public class FakeAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (user.IsInRole("admin"))
            {
                return Task.FromResult(AuthorizationResult.Success());
            }
            return Task.FromResult(AuthorizationResult.Failed());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            if (user.IsInRole("admin"))
            {
                return Task.FromResult(AuthorizationResult.Success());
            }
            return Task.FromResult(AuthorizationResult.Failed());
        }
    }
}
