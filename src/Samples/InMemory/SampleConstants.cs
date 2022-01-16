// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

internal class SampleConstants
{
    public static IEnumerable<ApiScope> ApiScopes { get; } = new List<ApiScope>
    {
        new ApiScope("api1", "My Api")
    };

    public static IEnumerable<Client> Clients { get; } = new List<Client>
    {
        new Client
        {
            ClientId = "client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes = { "api1" }
        }
    };
}