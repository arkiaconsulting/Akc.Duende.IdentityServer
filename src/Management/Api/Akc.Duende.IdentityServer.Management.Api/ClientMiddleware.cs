// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using IR = Microsoft.AspNetCore.Http.IResult;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal static class ClientMiddleware
    {
        public static Task<IR> GetAll([FromServices] IClientManagementStore store) =>
            store.GetAll()
            .Map(clients => clients.Select(DtoExtensions.FromModel))
            .Match(
                onSuccess: Results.Ok,
                onFailure: e => Results.BadRequest()
            );

        public static Task<IR> Get(string clientId, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Match(
                onSuccess: client => Results.Ok(client.FromModel()),
                onFailure: e => Results.NotFound()
            );

        public static Task<IR> Create(string clientId, ClientInputDto client, [FromServices] IClientManagementStore store, [FromServices] IOptions<ManagementApiOptions> options) =>
            store.Create(client.ToModel(clientId))
            .Match(
                onSuccess: () => Results.Created(HttpPipelineHelpers.FormatClientUri(clientId, options.Value), default),
                onFailure: e => Results.StatusCode((int)HttpStatusCode.InternalServerError)
            );

        public static Task<IR> Delete(string clientId, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Bind(c => store.Delete(clientId))
            .Match(
                onSuccess: () => Results.Ok(),
                onFailure: e => Results.NotFound()
            );

        public static Task<IR> Update(string clientId, ClientInputDto client, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Bind(_ => store.Update(clientId, client.ToModel(clientId)))
            .Match(
                onSuccess: () => Results.Ok(),
                onFailure: e => Results.NotFound()
            );

        public static Task<IR> GetSecret(string clientId, int id, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Bind(client => store.GetSecret(client.ClientId, id))
            .Map(model => model.ToDto(id))
            .Match(
                onSuccess: Results.Ok,
                onFailure: e => Results.NotFound()
            );

        public static Task<IR> AddSecret(string clientId, int id, CreateClientSecretInputDto clientSecret, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Ensure(client => store.GetSecret(client.ClientId, id).Match(_ => false, _ => true), Errors.ClientSecretAlreadyExist)
            .Bind(client => store.CreateSecret(clientId, clientSecret.ToModel(), id))
            .Match(
                onSuccess: () => Results.StatusCode((int)HttpStatusCode.Created),
                onFailure: e => e switch
                {
                    Errors.ClientNotFound => Results.BadRequest(),
                    Errors.ClientSecretAlreadyExist => Results.Ok(),
                    _ => Results.StatusCode((int)HttpStatusCode.InternalServerError)
                }
            );

        public static Task<IR> UpdateSecret(string clientId, int id, UpdateClientSecretInputDto clientSecret, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Bind(client => store.GetSecret(client.ClientId, id))
            .Tap(() => store.UpdateSecret(clientId, id, clientSecret.NewValue, clientSecret.Description, clientSecret.Expiration))
            .Match(
                onSuccess: _ => Results.Ok(),
                onFailure: e => e switch
                {
                    Errors.ClientSecretNotFound => Results.NotFound(),
                    Errors.ClientNotFound => Results.BadRequest(),
                    _ => Results.StatusCode((int)HttpStatusCode.InternalServerError)
                }
            );

        public static Task<IR> DeleteSecret(string clientId, int id, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Bind(client => store.GetSecret(client.ClientId, id))
            .Tap(() => store.DeleteSecret(clientId, id))
            .Match(
                onSuccess: _ => Results.Ok(),
                onFailure: e => e switch
                {
                    Errors.ClientNotFound => Results.BadRequest(),
                    Errors.ClientSecretNotFound => Results.NotFound(),
                    _ => Results.StatusCode((int)HttpStatusCode.InternalServerError)
                }
            );
    }
}