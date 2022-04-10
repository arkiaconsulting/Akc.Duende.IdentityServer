// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api.Tests.Assets;
using AutoFixture.Xunit2;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Akc.Duende.IdentityServer.Management.Api.Tests
{
    [Trait("Category", "Integration")]
    public class ClientManagementPipelineTests
    {
        private HttpClient Client { get; }
        private DefaultTestData TestData => _factory.Services.GetRequiredService<DefaultTestData>();
        private readonly DefaultWebApplicationFactory _factory;

        public ClientManagementPipelineTests()
        {
            _factory = new DefaultWebApplicationFactory();
            Client = _factory.CreateClient(new() { BaseAddress = new($"https://localhost{TestProgram.BasePath}/") });
        }

        [Fact(DisplayName = "Return stored clients")]
        [Trait("Category", "CLIENT")]
        public async Task Test01() =>
            (await Client.GetClients()).Should().BeEquivalentTo(
                TestData.Clients,
                opts => opts.Excluding(c => c.ClientSecrets));

        [Theory(DisplayName = "Add a new client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test02(ClientCreateDto client, string clientId)
        {
            using var _ = await Client.CreateClient(clientId, client);

            (await Client.GetClients()).Should().ContainEquivalentOf(client).Subject
                .ClientId.Should().Be(clientId.ToString());
        }

        [Theory(DisplayName = "Update an existing client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test03(ClientCreateDto existingClient, ClientUpdateDto updatedClient, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var _1 = await Client.UpdateClient(clientId, updatedClient);

            (await Client.GetClients()).Should().ContainEquivalentOf(updatedClient).Subject
                .ClientId.Should().Be(clientId.ToString());
        }

        [Theory(DisplayName = "Update a client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test04(ClientUpdateDto updatedClient, string clientId)
        {
            using var response = await Client.UpdateClient(clientId, updatedClient);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Delete an existing client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test05(ClientCreateDto existingClient, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var _1 = await Client.DeleteClient(clientId);

            (await Client.GetClients()).Should().NotContainEquivalentOf(existingClient);
        }

        [Theory(DisplayName = "Delete an existing client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test06(string clientId)
        {
            using var response = await Client.DeleteClient(clientId);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Pass when getting a client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test07(ClientCreateDto client, string clientId)
        {
            using var _ = await Client.CreateClient(clientId, client);

            var actualClient = await Client.GetClient(clientId);

            actualClient.Should().BeEquivalentTo(client);
            actualClient!.ClientId.Should().Be(clientId);
        }

        [Theory(DisplayName = "Fail when getting a client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test08(string clientId)
        {
            Func<Task> f = () => Client.GetClient(clientId);

            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory(DisplayName = "Pass when creating a simple client with client credentials")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test09(string clientId)
        {
            var client = new ClientCreateDto
            {
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new[] { IdentityServerConstants.LocalApi.ScopeName }
            };
            using var _ = await Client.CreateClient(clientId, client);

            var actualClient = await Client.GetClient(clientId);

            // Creating a new client using IS type, because of IS default values
            var expectedClient = new Client
            {
                ClientId = clientId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { IdentityServerConstants.LocalApi.ScopeName },
            };

            actualClient!.Should().BeEquivalentTo(expectedClient, opts =>
                opts.Excluding(c => c.ClientSecrets)
            );
        }
    }
}