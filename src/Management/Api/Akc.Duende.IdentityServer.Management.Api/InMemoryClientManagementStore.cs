// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class InMemoryClientManagementStore : IClientManagementStore
    {
        private readonly Dictionary<string, Client> _clients;
        private readonly IDictionary<string, ICollection<SecretInternal>> _secrets;
        public InMemoryClientManagementStore(IEnumerable<Client> clients)
        {
            _clients = (clients as ICollection<Client> ?? throw new InvalidOperationException($"Clients should be backed by a non fixed collection"))
                .ToDictionary(c => c.ClientId, c => c);
            _secrets = clients.ToDictionary(
                c => c.ClientId,
                c => (ICollection<SecretInternal>)c.ClientSecrets.Select((s, i) => SecretInternal.FromModel(c.ClientId, s, i)).ToList());
        }

        public Task<Result<IEnumerable<Client>>> GetAll() =>
            Result.Success(_clients.Values.AsEnumerable())
            .AsTask();

        public Task<Result<Client>> Get(string clientId) =>
            TryFindClient(_clients, clientId);

        public Task<Result> Create(Client client) =>
            Result.Success(_clients)
            .Tap(clients => { clients.Add(client.ClientId, client); _secrets.Add(client.ClientId, new List<SecretInternal>()); })
            .ForgetValue();

        public Task<Result> Delete(string clientId) =>
            TryFindClient(_clients, clientId)
            .Tap(client => _clients.Remove(client.ClientId))
            .ForgetValue();

        public Task<Result> Update(string clientId, Client client) =>
            TryFindClient(_clients, clientId)
            .Tap(existingClient => { _clients.Remove(existingClient.ClientId); _clients.Add(client.ClientId, client); })
            .ForgetValue();

        public Task<Result<Secret>> GetSecret(string clientId, int id) =>
            TryFindClient(_clients, clientId)
            .Bind(client => TryFindSecret(_secrets[client.ClientId], id));

        public Task<Result> CreateSecret(string clientId, Secret secret, int id) =>
            TryFindClient(_clients, clientId)
            .Tap(client => _secrets[client.ClientId].Add(SecretInternal.FromModel(client.ClientId, secret, id)))
            .ForgetValue();

        public Task<Result> UpdateSecret(string clientId, int id, string newValue) =>
            TryFindClient(_clients, clientId)
            .Bind(client => TryFindSecret(_secrets[client.ClientId], id))
            .Tap(secret => secret.Value = newValue)
            .ForgetValue();

        public Task<Result> DeleteSecret(string clientId, int id) =>
            TryFindClient(_clients, clientId)
            .Bind(client =>
                TryFindSecret(_secrets[client.ClientId], id)
                .Tap(secret => client.ClientSecrets.Remove(secret))
            ).ForgetValue();

        #region Private

        private static Task<Result<Client>> TryFindClient(Dictionary<string, Client> clients, string clientId) =>
            Result.Success(clients)
            .Bind(clients =>
                clients.TryFind(clientId)
                .ToResult(Errors.ClientNotFound)
            ).AsTask();

        private static Result<Secret> TryFindSecret(IEnumerable<SecretInternal> secrets, int id) =>
            secrets.TryFirst(s => s.Id == id)
            .Map(si => new Secret(si.Value, si.Description, si.Expiration) { Type = si.Type })
            .ToResult(Errors.ClientSecretNotFound);

        #endregion
    }
}
