using IdentityServer4.Models;
using IdentityServer4.Test;

namespace PreparedLogger.Auth.Web
{
    public static class Config
    {
        public static ApiResource[] GetApiResources()
        {
            return new ApiResource[]
            {
                new ApiResource("PreparedLogger", "API to interact with PreparedLogger backend")
            };
        }

        public static Client[] GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientId = "AdminConsole",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "PreparedLogger" }
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "PreparedLogger" }
                }
            };
        }

        public static TestUser[] GetUsers()
        {
            return new TestUser[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}