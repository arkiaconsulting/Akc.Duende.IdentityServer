// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Builder;

namespace Duende.IdentityServer.Akc.Management.Api
{
    public static class HttpPipelineExtensions
    {
        public static IApplicationBuilder AddIdentityServerClientApi(this IApplicationBuilder app) =>
            app.Map("/api/clients", appBuilder =>
            {
                appBuilder.UseMiddleware<ClientMiddleware>();
            });
    }
}