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

            return (client ?? Result.Failure<Client>(Errors.ClientNotFound)).AsTask();
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

        public Task<Result<Secret>> GetSecret(string clientId, string type, string value) =>
            Get(clientId)
            .Bind(client => _GetSecret(client.ClientSecrets, type, value));

        public Task<Result> CreateSecret(string clientId, Secret secret) =>
            Get(clientId)
            .Bind(client => _CreateSecret(client.ClientSecrets, secret));

        public Task<Result> UpdateSecret(string clientId, string type, string value, string newValue)
        {
            var client = Clients.Single(c => c.ClientId == clientId);
            var secret = client.ClientSecrets.Single(s => s.Type == type && s.Value == value);

            secret.Value = newValue;

            return Result.Success().AsTask();
        }

        public Task<Result> DeleteSecret(string clientId, string type, string value)
        {
            var client = Clients.Single(c => c.ClientId == clientId);
            var secret = client.ClientSecrets.Single(s => s.Type == type && s.Value == value);

            client.ClientSecrets.Remove(secret);

            return Result.Success().AsTask();
        }

        #region Private

        private Result<Secret> _GetSecret(IEnumerable<Secret> secrets, string type, string value)
        {
            var exist = ToHashSet(secrets).TryGetValue(new(value, default) { Type = type }, out var secret);

            return exist
                ? Result.Success(secret!)
                : Result.Failure<Secret>(Errors.ClientSecretNotFound);
        }

        private Result _CreateSecret(IEnumerable<Secret> secrets, Secret secret) =>
            ToHashSet(secrets).Add(secret) ? Result.Success() : Result.Failure("Cannot add secret");

        private HashSet<Secret> ToHashSet(IEnumerable<Secret> secrets) =>
            (secrets as HashSet<Secret>) ?? throw new InvalidOperationException("The secrets should be backed by a HashSet");

        #endregion
    }
}
