using IdentityModel;
using IdentityServer4.Models;

namespace Idp
{
    public class ProfileWithRoleIdentityResource : IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource() 
        {
            UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}
