// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Microsoft.Extensions.DependencyInjection;

namespace Akc.Duende.IdentityServer.Management.Api
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddInMemoryClientManagementApi(this IServiceCollection services) =>
            services.AddSingleton<IClientManagementStore, InMemoryClientManagementStore>();
    }
}
