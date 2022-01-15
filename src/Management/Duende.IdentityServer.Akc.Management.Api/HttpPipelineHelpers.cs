// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal static class HttpPipelineHelpers
    {
        public static string FormatClientUri(string clientId) =>
            string.Format("{0}/{1}", Constants.Paths.Clients, clientId);
    }
}
