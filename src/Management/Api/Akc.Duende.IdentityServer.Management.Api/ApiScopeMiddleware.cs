// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
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
                onSuccess: scope => Results.Ok(scope.ToDto()),
                onFailure: Results.NotFound
            );

        public static Task<IR> Create([FromRoute] string name, [FromBody] CreateUpdateApiScopeDto dto, [FromServices] IResourceManagementStore store, [FromServices] IOptions<ManagementApiOptions> options) =>
            store.Create(dto.ToModel(name))
            .Match(
                onSuccess: () => Results.Created(HttpPipelineHelpers.FormatClientUri(name, options.Value), default),
                onFailure: Results.BadRequest
            );

        public static Task<IR> Update([FromRoute] string name, [FromBody] CreateUpdateApiScopeDto dto, [FromServices] IResourceManagementStore store) =>
            store.Update(dto.ToModel(name))
            .Match(
                onSuccess: () => Results.Ok(),
                onFailure: Results.BadRequest
            );

        public static Task<IR> Delete([FromRoute] string name, [FromServices] IResourceManagementStore store) =>
            store.Delete(name)
            .Match(
                onSuccess: () => Results.Ok(),
                onFailure: Results.NotFound
            );
    }
}
