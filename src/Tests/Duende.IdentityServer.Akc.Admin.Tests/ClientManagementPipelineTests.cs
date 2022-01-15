// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using AutoFixture.Xunit2;
using FluentAssertions;
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
        private HttpClient Client => _factory.CreateClient();
        private JsonSerializerOptions JsonOptions => _factory.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;
        private DefaultTestData TestData => _factory.Services.GetRequiredService<DefaultTestData>();

        private readonly DefaultWebApplicationFactory _factory;

        public ClientManagementPipelineTests() =>
            _factory = new DefaultWebApplicationFactory();

        [Fact(DisplayName = "Return stored clients")]
        public async Task Test01()
        {
            var actualClients = await Client.GetFromJsonAsync<ClientDto[]>("api/clients");

            actualClients.Should().BeEquivalentTo(
                TestData.Clients,
                opts => opts.Excluding(c => c.ClientSecrets));
        }

        [Theory(DisplayName = "Add a new client")]
        [InlineAutoData]
        public async Task Test02(ClientCreateDto client, Guid clientId)
        {
            using var _ = await Client.PutAsJsonAsync($"api/clients/{clientId}", client, options: JsonOptions);

            var actualClients = await Client.GetFromJsonAsync<ClientDto[]>("api/clients");
            actualClients.Should().ContainEquivalentOf(client).Subject
                .ClientId.Should().Be(clientId.ToString());
        }

        [Theory(DisplayName = "Update an existing client")]
        [InlineAutoData]
        public async Task Test03(ClientCreateDto existingClient, ClientUpdateDto updatedClient, Guid clientId)
        {
            _ = await Client.PutAsJsonAsync($"api/clients/{clientId}", existingClient, options: JsonOptions);

            using var _1 = Client.PostAsJsonAsync($"api/clients/{clientId}", updatedClient, options: JsonOptions);

            var actualClients = await Client.GetFromJsonAsync<ClientDto[]>("api/clients", options: JsonOptions);
            actualClients.Should().ContainEquivalentOf(updatedClient).Subject
                .ClientId.Should().Be(clientId.ToString());
        }
    }
}