// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IR = Microsoft.AspNetCore.Http.IResult;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class ApiScopeMiddleware
    {
        public static Task<IR> Get([FromRoute] string name, [FromServices] IResourceManagementStore store) =>
            store.Get(name)
            .Match(
                onSuccess: scope => Results.Ok(new ApiScopeDto(scope.Name, scope.DisplayName, scope.Description, scope.ShowInDiscoveryDocument, scope.UserClaims.ToArray(), scope.Properties, scope.Enabled, scope.Required, scope.Emphasize)),
                onFailure: error => Results.BadRequest(error)
            );

        public static Task<IR> Create([FromRoute] string name, [FromBody] CreateUpdateApiScopeDto dto, [FromServices] IResourceManagementStore store, [FromServices] IOptions<ManagementApiOptions> options) =>
            store.Create(new ApiScope(name, dto.DisplayName, dto.UserClaims) { Description = dto.Description, Enabled = dto.Enabled, Properties = dto.Properties, Required = dto.Required, ShowInDiscoveryDocument = dto.ShowInDiscoveryDocument, Emphasize = dto.Emphasize })
            .Match(
                onSuccess: () => Results.Created(HttpPipelineHelpers.FormatClientUri(name, options.Value), default),
                onFailure: error => Results.BadRequest(error)
            );

        public static Task<IR> Update([FromRoute] string name, [FromBody] CreateUpdateApiScopeDto dto, [FromServices] IResourceManagementStore store) =>
            store.Update(new ApiScope(name, dto.DisplayName, dto.UserClaims) { Description = dto.Description, Enabled = dto.Enabled, Properties = dto.Properties, Required = dto.Required, ShowInDiscoveryDocument = dto.ShowInDiscoveryDocument, Emphasize = dto.Emphasize })
            .Match(
                onSuccess: () => Results.Ok(),
                onFailure: error => Results.BadRequest(error)
            );

        public static Task<IR> Delete([FromRoute] string name, [FromServices] IResourceManagementStore store) =>
            store.Delete(name)
            .Match(
                onSuccess: () => Results.Ok(),
                onFailure: error => Results.NotFound(error)
            );
    }
}
