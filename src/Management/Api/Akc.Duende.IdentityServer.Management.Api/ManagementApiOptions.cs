// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Http;

namespace Akc.Duende.IdentityServer.Management.Api
{
    public class ManagementApiOptions
    {
        public PathString BasePath { get; set; } = Constants.Paths.Clients;
        public string AuthorizationPolicy { get; set; } = "IdentityServerAccessToken";
    }
}
