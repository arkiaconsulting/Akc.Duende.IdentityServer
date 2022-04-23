// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal interface IApiResourceManagementStore
    {
        Task<Result> Create(ApiResource apiResource);
        Task<Result<ApiResource>> Get(string name);
        Task<Result> Update(ApiResource apiResource);
        Task<Result> Delete(string name);
    }
}
