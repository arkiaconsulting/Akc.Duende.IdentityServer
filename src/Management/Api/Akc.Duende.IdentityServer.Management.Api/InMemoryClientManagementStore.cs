// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class InMemoryClientManagementStore : IClientManagementStore
    {
        private readonly ICollection<Client> _inMemoryClients;
        public InMemoryClientManagementStore(IEnumerable<Client> clients) => _inMemoryClients = (clients as ICollection<Client>) ?? throw new InvalidOperationException($"Clients should be backed by a non fixed collection");//_secrets = clients.ToDictionary(//    c => c.ClientId,//    c => (ICollection<SecretInternal>)c.ClientSecrets.Select((s, i) => SecretInternal.FromModel(c.ClientId, s, i)).ToList());

        public Task<Result<IEnumerable<Client>>> GetAll() =>
            Result.Success(_inMemoryClients.AsEnumerable())
            .AsTask();

        public Task<Result<Client>> Get(string clientId) =>
            TryFindClient(_inMemoryClients, clientId);

        public Task<Result> Create(Client client) =>
            Result.Success(_inMemoryClients)
            .Tap(clients => { clients.Add(client); })
            .ForgetValue();

        public Task<Result> Delete(string clientId) =>
            TryFindClient(_inMemoryClients, clientId)
            .Tap(client => _inMemoryClients.Remove(client))
            .ForgetValue();

        public Task<Result> Update(string clientId, Client client) =>
            TryFindClient(_inMemoryClients, clientId)
            .Tap(existingClient => { _inMemoryClients.Remove(existingClient); _inMemoryClients.Add(client); })
            .ForgetValue();

        public Task<Result<Secret>> GetSecret(string clientId, int id) =>
            TryFindClient(_inMemoryClients, clientId)
            .Bind(client => TryFindSecret(client, id))
            .Map(secret => new Secret(secret.Value, secret.Description, secret.Expiration) { Type = secret.Type });

        public Task<Result> CreateSecret(string clientId, Secret secret, int id) =>
            TryFindClient(_inMemoryClients, clientId)
            .Tap(client => client.ClientSecrets.Add(SecretInternal.FromModel(secret, id)))
            .ForgetValue();

        public Task<Result> UpdateSecret(string clientId, int id, string newValue, string description, DateTime? expiration) =>
            TryFindClient(_inMemoryClients, clientId)
            .Bind(client =>
                TryFindSecret(client, id)
                .Tap(secret => { client.ClientSecrets.Remove(secret); client.ClientSecrets.Add(new SecretInternal(id, secret.Type, newValue, description, expiration)); })
            )
            .ForgetValue();

        public Task<Result> DeleteSecret(string clientId, int id) =>
            TryFindClient(_inMemoryClients, clientId)
            .Bind(client =>
                TryFindSecret(client, id)
                .Tap(secret => client.ClientSecrets.Remove(secret))
            ).ForgetValue();

        #region Private

        private static Task<Result<Client>> TryFindClient(IEnumerable<Client> clients, string clientId) =>
            Result.Success(clients)
            .Bind(clients =>
                clients.TryFirst(client => client.ClientId == clientId)
                .ToResult(Errors.ClientNotFound)
            ).AsTask();

        private static Result<SecretInternal> TryFindSecret(Client client, int id) =>
            client.ClientSecrets.OfType<SecretInternal>().TryFirst(s => s.Id == id)
            .ToResult(Errors.ClientSecretNotFound);

        #endregion
    }
}
