// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Duende.IdentityServer.Akc.Management.Api
{
    public static class HttpPipelineExtensions
    {
        public static IEndpointRouteBuilder AddIdentityServerClientApi(this IEndpointRouteBuilder app)
        {
            app.MapGet(Constants.Paths.Clients, ClientMiddleware.Get);
            app.MapPut($"{Constants.Paths.Clients}/{{clientId}}", ClientMiddleware.Create);
            app.MapPost($"{Constants.Paths.Clients}/{{clientId}}", ClientMiddleware.Update);
            app.MapDelete($"{Constants.Paths.Clients}/{{clientId}}", ClientMiddleware.Delete);

            return app;
        }
    }
}