// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Akc.Management.Api;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddInMemoryClientManagementApi(this IServiceCollection services) =>
            services.AddSingleton<IClientManagementStore, InMemoryClientManagementStore>();
    }
}
