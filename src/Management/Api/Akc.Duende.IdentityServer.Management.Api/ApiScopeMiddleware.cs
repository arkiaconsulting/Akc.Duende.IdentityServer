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
        private static readonly IDictionary<string, ApiScopeDto> Store = new Dictionary<string, ApiScopeDto>();

        public static Task<IR> GetAll() =>
            Results.Ok(new object()).AsTask();

        public static Task<IR> Get(string name) =>
            Result.SuccessIf(Store.ContainsKey(name), Store, "api scope not found")
            .Bind(store => Result.Success(store[name]))
            .Match(
                onSuccess: dto => Results.Ok(dto),
                onFailure: error => Results.BadRequest(error)
            )
            .AsTask();

        public static Task<IR> Create(string name, CreateUpdateApiScopeDto dto, [FromServices] IOptions<ManagementApiOptions> options) =>
            Result.SuccessIf(!Store.ContainsKey(name), Store, "An Api scope with this name already exists")
            .Tap(store =>
                store.Add(name,
                new ApiScopeDto(name, dto.DisplayName, dto.Description, dto.ShowInDiscoveryDocument, dto.UserClaims, dto.Properties, dto.Enabled, dto.Required)
                )
            )
            .Match(
                onSuccess: _ => Results.Created(HttpPipelineHelpers.FormatClientUri(name, options.Value), default),
                onFailure: error => Results.BadRequest(error)
            )
            .AsTask();

        public static Task<IR> Update(string name, CreateUpdateApiScopeDto dto) =>
            Result.SuccessIf(Store.ContainsKey(name), Store, "An Api scope with this name cannot be found")
            .Tap(store => { store.Remove(name); store.Add(name, new ApiScopeDto(name, dto.DisplayName, dto.Description, dto.ShowInDiscoveryDocument, dto.UserClaims, dto.Properties, dto.Enabled, dto.Required)); })
            .Match(
                onSuccess: _ => Results.Ok(),
                onFailure: error => Results.BadRequest(error)
            ).AsTask();

        public static Task<IR> Delete(string name) =>
            Result.SuccessIf(Store.ContainsKey(name), Store, "An Api scope with this name cannot be found")
            .Tap(store => store.Remove(name))
            .Match(
                onSuccess: _ => Results.Ok(),
                onFailure: error => Results.NotFound(error)
            )
            .AsTask();
    }
}
