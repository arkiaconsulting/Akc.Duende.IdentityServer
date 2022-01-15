// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Akc.Admin.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Duende.IdentityServer.Akc.Admin.Tests
{
    internal class TestProgram
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var testData = new DefaultTestData();

            builder.Services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(testData.Clients)
                .AddInMemoryIdentityResources(testData.IdentityResources);

            builder.Services.AddSingleton(testData);

            var app = builder.Build();

            app.UseIdentityServer();

            app.AddIdentityServerClientApi();

            app.Run();
        }
    }
}