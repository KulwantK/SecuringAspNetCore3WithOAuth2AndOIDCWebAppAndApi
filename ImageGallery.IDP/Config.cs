using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[] { };

        public static IEnumerable<Client> Clients =>
            new Client[]
            { 
                new Client
                {
                    ClientName="Image Gallery",
                    ClientId="imagegalleryclient",
                    AllowedGrantTypes=GrantTypes.Code,
                    RequirePkce=true,
                    RedirectUris=new List<string>
                    {
                        "https://localhost:44358/signin-oidc"
                    },
                    PostLogoutRedirectUris=new List<string>
                    {
                        "https://localhost:44358/signout-callback-oidc"
                    },
                    AllowedScopes=
                    { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    ClientSecrets=
                    {
                        new Secret("seceret".Sha256())
                    }
                }
            };
    }
}
