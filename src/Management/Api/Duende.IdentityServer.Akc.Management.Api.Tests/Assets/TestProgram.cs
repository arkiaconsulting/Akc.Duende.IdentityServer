// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Duende.IdentityServer.Akc.Management.Api.Tests.Assets
{
    internal class TestProgram
    {
        public const string BasePath = "/my/clients";

        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
                Args = args
            });

            var testData = new DefaultTestData();

            builder.Services.Configure<ManagementApiOptions>(options =>
            {
                options.BasePath = BasePath;
            });

            builder.Services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(testData.Clients)
                .AddInMemoryIdentityResources(testData.IdentityResources);

            builder.Services.AddSingleton(testData);

            var app = builder.Build();

            app.UseIdentityServer();

            app.UseIdentityServerClientApi();

            app.Run();
        }
    }
}