// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static IIdentityServerBuilder AddInMemoryManagementApi(this IIdentityServerBuilder builder)
        {
            builder.Services
            .AddTransient<IClientManagementStore, InMemoryClientManagementStore>()
            .AddTransient<IResourceManagementStore, InMemoryResourceManagementStore>()
            .AddTransient<IApiResourceManagementStore, InMemoryApiResourceManagementStore>();

            return builder;
        }
    }
}
