// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Duende.IdentityServer.Akc.Management.Api
{
    public static class HttpPipelineExtensions
    {
        public static IEndpointRouteBuilder UseIdentityServerClientApi(this IEndpointRouteBuilder app)
        {
            var options = app.ServiceProvider.GetRequiredService<IOptions<ManagementApiOptions>>().Value;

            app.MapGet(options.BasePath, ClientMiddleware.Get);
            app.MapPut($"{options.BasePath}/{{clientId}}", ClientMiddleware.Create);
            app.MapPost($"{options.BasePath}/{{clientId}}", ClientMiddleware.Update);
            app.MapDelete($"{options.BasePath}/{{clientId}}", ClientMiddleware.Delete);

            app.MapPut($"{options.BasePath}/{{clientId}}/secrets", ClientMiddleware.AddSecret);
            app.MapPost($"{options.BasePath}/{{clientId}}/secrets", ClientMiddleware.UpdateSecret);
            app.MapDelete($"{options.BasePath}/{{clientId}}/secrets/{{type}}/{{value}}", ClientMiddleware.DeleteSecret);

            return app;
        }
    }
}