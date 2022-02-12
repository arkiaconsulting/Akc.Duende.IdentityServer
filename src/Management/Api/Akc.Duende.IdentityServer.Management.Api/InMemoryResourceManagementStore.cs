// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class InMemoryResourceManagementStore : IResourceManagementStore
    {
        private readonly ICollection<ApiScope> _apiScopes;

        public InMemoryResourceManagementStore(IEnumerable<ApiScope> apiScopes) =>
            _apiScopes = (apiScopes as ICollection<ApiScope>) ?? throw new InvalidOperationException($"Clients should be backed by a non fixed collection");

        Task<Result> IResourceManagementStore.Create(ApiScope apiScope) =>
            EnsureNotExist(_apiScopes, apiScope.Name)
            .Tap(() => _apiScopes.Add(apiScope))
            .AsTask();

        Task<Result> IResourceManagementStore.Delete(string name) =>
            TryFind(_apiScopes, name)
            .Tap(scope => _apiScopes.Remove(scope))
            .ForgetValue();

        Task<Result<ApiScope>> IResourceManagementStore.Get(string name) =>
            TryFind(_apiScopes, name)
            .AsTask();

        Task<Result> IResourceManagementStore.Update(ApiScope apiScope) =>
            TryFind(_apiScopes, apiScope.Name)
            .Tap(scope => { _apiScopes.Remove(scope); _apiScopes.Add(apiScope); })
            .ForgetValue();

        #region Private

        private static Result<ApiScope> TryFind(IEnumerable<ApiScope> apiScopes, string name) =>
            Result.SuccessIf(
                apiScopes.Any(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)),
                apiScopes,
                "An Api scope with this name cannot be found"
            )
            .Map(source => source.First(scope => scope.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));

        private static Result EnsureNotExist(IEnumerable<ApiScope> apiScopes, string name) =>
            Result.FailureIf(
                apiScopes.Any(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)),
                "An Api scope with this name already exist"
            );

        #endregion
    }
}
