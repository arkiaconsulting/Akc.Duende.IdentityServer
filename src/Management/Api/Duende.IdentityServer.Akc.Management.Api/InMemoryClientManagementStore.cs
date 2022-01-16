// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal class InMemoryClientManagementStore : IClientManagementStore
    {
        private ICollection<Client> Clients => _clients
            ?? throw new InvalidOperationException($"Clients should be backed by a non fixed collection");

        private readonly ICollection<Client>? _clients;

        public InMemoryClientManagementStore(IEnumerable<Client> clients) =>
            _clients = clients as ICollection<Client>;

        public Task<Result<Client>> Get(string clientId)
        {
            var client = Clients.SingleOrDefault(c => c.ClientId == clientId);

            return (client ?? Result.Failure<Client>($"No client found with Id '{clientId}'")).AsTask();
        }

        public Task<Result> Create(Client client)
        {
            Clients.Add(client);

            return Result.Success().AsTask();
        }

        public Task<Result> Delete(string clientId)
        {
            var client = Clients.Single(c => c.ClientId == clientId);

            Clients.Remove(client);

            return Result.Success().AsTask();
        }

        public Task<Result> Update(string clientId, Client client)
        {
            var existingClient = Clients.Single(c => c.ClientId == clientId);

            Clients.Remove(existingClient);

            Clients.Add(client);

            return Result.Success().AsTask();
        }
    }
}
