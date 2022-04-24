// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal interface IClientManagementStore
    {
        Task<Result<Client>> Get(string clientId);
        Task<Result<IEnumerable<Client>>> GetAll();
        Task<Result> Create(Client client);
        Task<Result> Delete(string clientId);
        Task<Result> Update(string clientId, Client client);
        Task<Result<Secret>> GetSecret(string clientId, string name);
        Task<Result> CreateSecret(string clientId, Secret secret, string name);
        Task<Result> UpdateSecret(string clientId, string name, string newValue, string description, DateTime? expiration);
        Task<Result> DeleteSecret(string clientId, string name);
    }
}
