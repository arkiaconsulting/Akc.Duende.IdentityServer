// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using IR = Microsoft.AspNetCore.Http.IResult;

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal class ClientMiddleware
    {
        public static Task<IEnumerable<ClientOutputDto>> GetAll(IEnumerable<Client> clients)
        {
            var dto = clients.Select(DtoExtensions.FromModel);

            return dto.AsTask();
        }

        public static Task<IR> Get(string clientId, [FromServices] IClientManagementStore store) =>
            store.Get(clientId)
            .Match(
                onSuccess: client => Results.Ok(client.FromModel()),
                onFailure: e => Results.BadRequest()
            );

        public static Task<IR> Create(string clientId, ClientInputDto client, [FromServices] IClientManagementStore store, [FromServices] IOptions<ManagementApiOptions> options) =>
            store.Create(client.ToModel(clientId))
            .Match(
                onSuccess: () => Results.Created(HttpPipelineHelpers.FormatClientUri(clientId, options.Value), default),
                onFailure: e => throw new NotImplementedException()
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

        public static Task<IR> AddSecret(string clientId, CreateClientSecretInputDto clientSecret, IEnumerable<Client> clients)
        {
            var store = EnsureCollection(clients);
            try
            {
                var client = store.Single(c => c.ClientId == clientId);

                var created = (client.ClientSecrets as HashSet<Secret>)!.Add(clientSecret.ToModel());

                return !created
                    ? Results.StatusCode((int)HttpStatusCode.OK).AsTask()
                    : Results.StatusCode((int)HttpStatusCode.Created).AsTask();
            }
            catch (InvalidOperationException)
            {
                return Results.StatusCode((int)HttpStatusCode.BadRequest).AsTask();
            }
        }

        public static Task<IR> UpdateSecret(string clientId, UpdateClientSecretInputDto clientSecret, IEnumerable<Client> clients)
        {
            var store = EnsureCollection(clients);

            try
            {
                var client = store.Single(c => c.ClientId == clientId);
                if (!(client.ClientSecrets as HashSet<Secret>)!.TryGetValue(new Secret(clientSecret.Value) { Type = clientSecret.Type }, out var existingSecret))
                {
                    return Results.NotFound().AsTask();
                }

                existingSecret.Value = clientSecret.NewValue;

                return Results.Ok().AsTask();
            }
            catch (InvalidOperationException)
            {
                return Results.BadRequest().AsTask();
            }
        }

        public static Task<IR> DeleteSecret(string clientId, string type, string value, IEnumerable<Client> clients)
        {
            var store = EnsureCollection(clients);

            try
            {
                var client = store.Single(c => c.ClientId == clientId);
                if (!(client.ClientSecrets as HashSet<Secret>)!.TryGetValue(new Secret(value) { Type = type }, out var existingSecret))
                {
                    return Results.NotFound().AsTask();
                }

            (client.ClientSecrets as HashSet<Secret>)!.Remove(existingSecret);

                return Results.Ok().AsTask();
            }
            catch (InvalidOperationException)
            {
                return Results.BadRequest().AsTask();
            }
        }

        #region Private

        private static ICollection<Client> EnsureCollection(IEnumerable<Client> clients) =>
            clients is not ICollection<Client> clientStore
            ? throw new InvalidOperationException()
            : clientStore;

        #endregion
    }
}