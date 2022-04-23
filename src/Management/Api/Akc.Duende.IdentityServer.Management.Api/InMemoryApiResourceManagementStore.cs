// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;
using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class InMemoryApiResourceManagementStore : IApiResourceManagementStore
    {
        private readonly ICollection<ApiResource> _apiResources;

        public InMemoryApiResourceManagementStore(IEnumerable<ApiResource> apiResources) =>
            _apiResources = (apiResources as ICollection<ApiResource>) ?? throw new InvalidOperationException($"Api resources should be backed by a non fixed collection");

        Task<Result> IApiResourceManagementStore.Create(ApiResource apiResource) =>
            EnsureNotExist(_apiResources, apiResource.Name)
            .Tap(() => _apiResources.Add(apiResource))
            .AsTask();

        Task<Result<ApiResource>> IApiResourceManagementStore.Get(string name) =>
            TryFind(_apiResources, name)
            .AsTask();

        Task<Result> IApiResourceManagementStore.Update(ApiResource apiResource) =>
            TryFind(_apiResources, apiResource.Name)
            .Tap(scope => { _apiResources.Remove(scope); _apiResources.Add(apiResource); })
            .ForgetValue();

        Task<Result> IApiResourceManagementStore.Delete(string name) =>
            TryFind(_apiResources, name)
            .Tap(scope => _apiResources.Remove(scope))
            .ForgetValue();

        #region Private

        private static Result<ApiResource> TryFind(IEnumerable<ApiResource> apiResources, string name) =>
            Result.SuccessIf(
                apiResources.Any(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)),
                apiResources,
                "An Api resource with this name cannot be found"
            )
            .Map(source => source.First(scope => scope.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));

        private static Result EnsureNotExist(IEnumerable<ApiResource> apiResources, string name) =>
            Result.FailureIf(
                apiResources.Any(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)),
                "An Api resource with this name already exist"
            );

        #endregion
    }
}
