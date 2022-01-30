// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal class InMemoryClientManagementStore : IClientManagementStore
    {
        private readonly ICollection<Client> _clients;

        public InMemoryClientManagementStore(IEnumerable<Client> clients) =>
            _clients = clients as ICollection<Client> ?? throw new InvalidOperationException($"Clients should be backed by a non fixed collection");

        public Task<Result<Client>> Get(string clientId) =>
            TryFindClient(_clients, clientId);

        public Task<Result> Create(Client client) =>
            Result.Success(_clients)
            .Tap(clients => clients.Add(client))
            .ForgetValue();

        public Task<Result> Delete(string clientId) =>
            TryFindClient(_clients, clientId)
            .Tap(client => _clients.Remove(client))
            .ForgetValue();

        public Task<Result> Update(string clientId, Client client) =>
            TryFindClient(_clients, clientId)
            .Tap(existingClient => { _clients.Remove(existingClient); _clients.Add(client); })
            .ForgetValue();

        public Task<Result<Secret>> GetSecret(string clientId, string type, string value) =>
            TryFindClient(_clients, clientId)
            .Bind(client => TryFindSecret(client.ClientSecrets, type, value));

        public Task<Result> CreateSecret(string clientId, Secret secret) =>
            TryFindClient(_clients, clientId)
            .Bind(client =>
                ToHashSet(client.ClientSecrets).Add(secret)
                ? Result.Success()
                : Result.Failure(Errors.ClientSecretAlreadyExist)
            );

        public Task<Result> UpdateSecret(string clientId, string type, string value, string newValue) =>
            TryFindClient(_clients, clientId)
            .Bind(client =>
                TryFindSecret(client.ClientSecrets, type, value)
                .Tap(secret => secret.Value = newValue)
            ).ForgetValue();

        public Task<Result> DeleteSecret(string clientId, string type, string value) =>
            TryFindClient(_clients, clientId)
            .Bind(client =>
                TryFindSecret(client.ClientSecrets, type, value)
                .Tap(secret => client.ClientSecrets.Remove(secret))
            ).ForgetValue();

        #region Private

        private static Task<Result<Client>> TryFindClient(ICollection<Client> clients, string clientId) =>
            Result.Success(clients)
            .Bind(clients =>
                clients.TryFirst(client => client.ClientId == clientId)
                .ToResult(Errors.ClientNotFound)
            ).AsTask();

        private static Result<Secret> TryFindSecret(IEnumerable<Secret> secrets, string type, string value) =>
            secrets.TryFirst(s => s.Type == type && s.Value == value)
            .ToResult(Errors.ClientSecretNotFound);

        private static HashSet<Secret> ToHashSet(IEnumerable<Secret> secrets) =>
            (secrets as HashSet<Secret>) ?? throw new InvalidOperationException("The secrets should be backed by a HashSet");

        #endregion
    }
}
