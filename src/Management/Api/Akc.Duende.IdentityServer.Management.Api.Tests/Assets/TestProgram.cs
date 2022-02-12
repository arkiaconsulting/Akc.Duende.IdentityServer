// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Akc.Duende.IdentityServer.Management.Api.Tests.Assets
{
    internal class TestProgram
    {
        public const string BasePath = "/my";

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
                .AddInMemoryIdentityResources(testData.IdentityResources)
                .AddInMemoryManagementApi();

            builder.Services.AddLocalApiAuthentication();

            builder.Services.AddIdentityServerLocalJwtBearerByPass(); // for tests purpose

            builder.Services.AddSingleton(testData);

            var app = builder.Build();

            app.UseIdentityServer();
            app.UseAuthorization();

            app
            .UseIdentityServerManagementApi()
            .RequireAuthorization(IdentityServerConstants.LocalApi.PolicyName);

            app.Run();
        }
    }
}