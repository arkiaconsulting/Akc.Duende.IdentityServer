// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Akc.Admin.Api;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Duende.IdentityServer.Akc.Admin.Tests
{
    internal class TestProgram
    {

        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(new List<Client>(DefaultTestData.Clients))
                .AddInMemoryIdentityResources(DefaultTestData.IdentityResources);

            var app = builder.Build();

            app.UseIdentityServer();

            app.AddIdentityServerClientApi();

            app.Run();
        }
    }
}