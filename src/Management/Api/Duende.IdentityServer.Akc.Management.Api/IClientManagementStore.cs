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
    }
}
