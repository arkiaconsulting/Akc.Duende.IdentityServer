// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal class InMemoryClientManagementStore : IClientManagementStore
    {
        private readonly ICollection<Client> _clients;

        public InMemoryClientManagementStore(IEnumerable<Client> clients)
        {
            if (clients is not ICollection<Client> collection)
            {
                throw new InvalidOperationException();
            }

            _clients = collection;
        }

        public Task<Result<Client>> Get(string clientId)
        {
            var client = _clients.SingleOrDefault(c => c.ClientId == clientId);

            return (client ?? Result.Failure<Client>($"No client found with Id '{clientId}'")).AsTask();
        }

        public Task<Result> Create(Client client)
        {
            _clients.Add(client);

            return Result.Success().AsTask();
        }
    }
}
