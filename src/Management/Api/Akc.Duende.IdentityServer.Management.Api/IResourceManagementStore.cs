// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal interface IResourceManagementStore
    {
        Task<Result<ApiScope>> Get(string name);
        Task<Result> Create(ApiScope apiScope);
        Task<Result> Delete(string name);
        Task<Result> Update(ApiScope apiScope);
    }
}
