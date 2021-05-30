// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                // let's include the role claim in the profile
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()

            };

        public static IEnumerable<ApiResource> Apis =>
           new ApiResource[]
           {
                // the api requires the role claim
                new ApiResource("rapidblazor.api.api", "RapidBlazor API", new[] { JwtClaimTypes.Role })
           };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("rapidblazor.api.scope", new [] { 
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Profile,
                    JwtClaimTypes.Name,
                    JwtClaimTypes.GivenName,
                    JwtClaimTypes.FamilyName,
                    JwtClaimTypes.Role
                })
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "rapidblazor.webui.client",
                    ClientName = "Rapid Blazor WebUI Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris = { "https://localhost:5503/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:5503/authentication/logout-callback" },
                    AllowedScopes = { "openid", "profile", "email" , "rapidblazor.api.scope" },
                    AllowedCorsOrigins = { "https://localhost:5503" },
                    RequireConsent = false,
                    Enabled = true
                }
            };
    }
}