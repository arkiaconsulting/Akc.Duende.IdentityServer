// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Akc.Duende.IdentityServer.Management.Api
{
    public static class HttpPipelineExtensions
    {
        public static IdentityServerClientApiEndpointBuilder UseIdentityServerManagementApi(this IEndpointRouteBuilder app)
        {
            var options = app.ServiceProvider.GetRequiredService<IOptions<ManagementApiOptions>>().Value;
            var builders = new List<RouteHandlerBuilder>
            {
                app.MapGet($"{options.BasePath}/clients", ClientMiddleware.GetAll),
                app.MapGet($"{options.BasePath}/clients/{{clientId}}", ClientMiddleware.Get),
                app.MapPut($"{options.BasePath}/clients/{{clientId}}", ClientMiddleware.Create),
                app.MapPost($"{options.BasePath}/clients/{{clientId}}", ClientMiddleware.Update),
                app.MapDelete($"{options.BasePath}/clients/{{clientId}}", ClientMiddleware.Delete),

                app.MapGet($"{options.BasePath}/clients/{{clientId}}/secrets/{{id}}", ClientMiddleware.GetSecret),
                app.MapPut($"{options.BasePath}/clients/{{clientId}}/secrets", ClientMiddleware.AddSecret),
                app.MapPost($"{options.BasePath}/clients/{{clientId}}/secrets", ClientMiddleware.UpdateSecret),
                app.MapDelete($"{options.BasePath}/clients/{{clientId}}/secrets/{{id}}", ClientMiddleware.DeleteSecret),

                app.MapGet($"{options.BasePath}/scopes/{{name}}", ApiScopeMiddleware.Get),
                app.MapPut($"{options.BasePath}/scopes/{{name}}", ApiScopeMiddleware.Create),
                app.MapPost($"{options.BasePath}/scopes/{{name}}", ApiScopeMiddleware.Update),
                app.MapDelete($"{options.BasePath}/scopes/{{name}}", ApiScopeMiddleware.Delete),
            };

            return new(app, builders);
        }

        public static IdentityServerClientApiEndpointBuilder RequireAuthorization(
            this IdentityServerClientApiEndpointBuilder builder,
            string policy)
        {
            builder.RouteHandlerBuilders.ToList().ForEach(builder => builder.RequireAuthorization(policy));

            return builder;
        }

        public class IdentityServerClientApiEndpointBuilder
        {
            public IEndpointRouteBuilder App { get; }
            public IEnumerable<RouteHandlerBuilder> RouteHandlerBuilders { get; } = new List<RouteHandlerBuilder>();

            public IdentityServerClientApiEndpointBuilder(
                IEndpointRouteBuilder app,
                IEnumerable<RouteHandlerBuilder> routeHandlerBuilders)
            {
                App = app;
                RouteHandlerBuilders = routeHandlerBuilders;
            }
        }
    }
}