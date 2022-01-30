// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal static class HttpPipelineHelpers
    {
        public static string FormatClientUri(string clientId, ManagementApiOptions options) =>
            string.Format("{0}/{1}", options.BasePath, clientId);
    }
}
