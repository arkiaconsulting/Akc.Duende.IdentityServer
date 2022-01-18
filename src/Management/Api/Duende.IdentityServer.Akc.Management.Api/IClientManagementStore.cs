// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal interface IClientManagementStore
    {
        Task<Result<Client>> Get(string clientId);
        Task<Result> Create(Client client);
        Task<Result> Delete(string clientId);
        Task<Result> Update(string clientId, Client client);
        Task<Result<Secret>> GetSecret(string clientId, string type, string value);
        Task<Result> CreateSecret(string clientId, Secret secret);
        Task<Result> UpdateSecret(string clientId, string type, string value, string newValue);
    }
}
