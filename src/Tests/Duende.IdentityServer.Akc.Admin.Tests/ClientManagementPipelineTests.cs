// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using AutoFixture.Xunit2;
using Duende.IdentityServer.Akc.Management.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Duende.IdentityServer.Akc.Management.Tests
{
    [Trait("Category", "Integration")]
    public class ClientManagementPipelineTests
    {
        private HttpClient Client { get; }
        private JsonSerializerOptions JsonOptions => _factory.Services.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
        private DefaultTestData TestData => _factory.Services.GetRequiredService<DefaultTestData>();

        private readonly DefaultWebApplicationFactory _factory;

        public ClientManagementPipelineTests()
        {
            _factory = new DefaultWebApplicationFactory();
            Client = _factory.CreateClient(new() { BaseAddress = new($"https://localhost{Constants.Paths.Clients}/") });
        }

        [Fact(DisplayName = "Return stored clients")]
        public async Task Test01()
        {
            var actualClients = await Client.GetFromJsonAsync<ClientDto[]>(string.Empty);

            actualClients.Should().BeEquivalentTo(
                TestData.Clients,
                opts => opts.Excluding(c => c.ClientSecrets));
        }

        [Theory(DisplayName = "Add a new client")]
        [InlineAutoData]
        public async Task Test02(ClientCreateDto client, Guid clientId)
        {
            using var _ = await Client.PutAsJsonAsync($"{clientId}", client);

            var actualClients = await Client.GetFromJsonAsync<ClientDto[]>(string.Empty);
            actualClients.Should().ContainEquivalentOf(client).Subject
                .ClientId.Should().Be(clientId.ToString());
        }

        [Theory(DisplayName = "Update an existing client")]
        [InlineAutoData]
        public async Task Test03(ClientCreateDto existingClient, ClientUpdateDto updatedClient, Guid clientId)
        {
            _ = await Client.PutAsJsonAsync($"{clientId}", existingClient);

            using var _1 = await Client.PostAsJsonAsync($"{clientId}", updatedClient);

            var actualClients = await Client.GetFromJsonAsync<ClientDto[]>(string.Empty);
            actualClients.Should().ContainEquivalentOf(updatedClient).Subject
                .ClientId.Should().Be(clientId.ToString());
        }

        [Theory(DisplayName = "Update a client that does not exist")]
        [InlineAutoData]
        public async Task Test031(ClientUpdateDto updatedClient, Guid clientId)
        {
            using var response = await Client.PostAsJsonAsync($"{clientId}", updatedClient);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory(DisplayName = "Delete an existing client")]
        [InlineAutoData]
        public async Task Test04(ClientCreateDto existingClient, Guid clientId)
        {
            _ = await Client.PutAsJsonAsync($"{clientId}", existingClient);

            using var _1 = await Client.DeleteAsync($"{clientId}");

            var actualClients = await Client.GetFromJsonAsync<ClientDto[]>(string.Empty);
            actualClients.Should().NotContainEquivalentOf(existingClient);
        }

        [Theory(DisplayName = "Delete an existing client that does not exist")]
        [InlineAutoData]
        public async Task Test041(Guid clientId)
        {
            using var response = await Client.DeleteAsync($"{clientId}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}