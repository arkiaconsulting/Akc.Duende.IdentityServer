// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

namespace Akc.Duende.IdentityServer.Management.Api
{
    public static class Constants
    {
        public static class Paths
        {
            public const string DefaultBasePath = "/api";

            internal static class SubPaths
            {
                public const string Clients = "clients";
                public const string ClientSecrets = "secrets";
                public const string ApiScopes = "scopes";
                public const string ApiResources = "resources";
            }
        }
    }
}
