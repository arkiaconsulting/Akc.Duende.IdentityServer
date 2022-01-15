// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal class ClientMiddleware
    {
        public static Task<IEnumerable<ClientOutputDto>> Get(IEnumerable<Client> clients)
        {
            var dto = clients.Select(DtoExtensions.FromModel);

            return dto.AsTask();
        }

        public static Task<IResult> Create(string clientId, ClientInputDto client, IEnumerable<Client> clients)
        {
            var store = EnsureCollection(clients);

            store.Add(client.ToModel(clientId));

            return Results.Created(HttpPipelineHelpers.FormatClientUri(clientId), default).AsTask();
        }

        public static Task<IResult> Delete(string clientId, IEnumerable<Client> clients)
        {
            var store = EnsureCollection(clients);

            try
            {
                var client = store.Single(c => c.ClientId == clientId);

                store.Remove(client);
            }
            catch (InvalidOperationException)
            {
                // Intentionally left empty
            }

            return Results.Ok().AsTask();
        }

        public static Task<IResult> Update(string clientId, ClientInputDto client, IEnumerable<Client> clients)
        {
            var store = EnsureCollection(clients);

            try
            {
                var existingClient = store.First(x => x.ClientId == clientId);
                store.Remove(existingClient);

                store.Add(client.ToModel(clientId));

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