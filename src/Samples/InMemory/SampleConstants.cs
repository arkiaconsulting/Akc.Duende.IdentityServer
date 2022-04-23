// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer;
using Duende.IdentityServer.Models;

internal class SampleConstants
{
    public static IEnumerable<ApiScope> ApiScopes { get; } = new List<ApiScope>
    {
        new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
    };

    public static IEnumerable<ApiResource> ApiResources { get; } = new List<ApiResource>
    {
        new ApiResource("api1", "Api 1") { Scopes = { "api1_read" }}
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
            AllowedScopes = { IdentityServerConstants.LocalApi.ScopeName }
        }
    };
}