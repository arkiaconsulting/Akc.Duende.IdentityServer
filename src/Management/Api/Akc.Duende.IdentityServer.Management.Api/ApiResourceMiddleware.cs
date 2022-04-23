// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IR = Microsoft.AspNetCore.Http.IResult;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class ApiResourceMiddleware
    {
        public static Task<IR> Get([FromRoute] string name, [FromServices] IApiResourceManagementStore store) =>
            store.Get(name)
            .Match(
                onSuccess: apiResource => Results.Ok(apiResource.ToDto()),
                onFailure: Results.NotFound
            );

        public static Task<IR> Create([FromRoute] string name, [FromBody] CreateUpdateApiResourceDto dto, [FromServices] IApiResourceManagementStore store, [FromServices] IOptions<ManagementApiOptions> options) =>
            store.Create(dto.ToModel(name))
            .Match(
                onSuccess: () => Results.Created(HttpPipelineHelpers.FormatClientUri(name, options.Value), default),
                onFailure: Results.BadRequest
            );
    }
}
