// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api.Tests.Assets;
using AutoFixture.Xunit2;
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
        public async Task Test031(ClientUpdateDto updatedClient, string clientId)
        {
            using var response = await Client.UpdateClient(clientId, updatedClient);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Delete an existing client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test04(ClientCreateDto existingClient, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var _1 = await Client.DeleteClient(clientId);

            (await Client.GetClients()).Should().NotContainEquivalentOf(existingClient);
        }

        [Theory(DisplayName = "Delete an existing client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test041(string clientId)
        {
            using var response = await Client.DeleteClient(clientId);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Add a new client secret to an existing client")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test05(ClientCreateDto existingClient, CreateClientSecretDto newClientSecret, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var response = await Client.CreateClientSecret(clientId, newClientSecret);

            response.Should().Be201Created();
        }

        [Theory(DisplayName = "Fail adding a client secret when the client does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test051(string clientId, CreateClientSecretDto newClientSecret)
        {
            using var response = await Client.CreateClientSecret(clientId, newClientSecret);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Pass when adding a client secret that already exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test052(ClientCreateDto existingClient, CreateClientSecretDto newClientSecret, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, newClientSecret);

            using var response = await Client.CreateClientSecret(clientId, newClientSecret);

            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Pass when updating a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test053(ClientCreateDto existingClient, CreateClientSecretDto initialClientSecret, string clientId, string newSecret, string description, DateTime newExpiry)
        {
            var updateSecretDto = new UpdateClientSecretDto(initialClientSecret.Id, newSecret, description, newExpiry);
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, initialClientSecret);

            using var response = await Client.UpdateClientSecret(clientId, updateSecretDto);

            response.Should().Be200Ok();
            var actualClientSecret = await Client.GetClientSecret(clientId, initialClientSecret.Id);
            actualClientSecret.Should().NotBeNull().And
                .BeEquivalentTo(new
                {
                    initialClientSecret.Id,
                    initialClientSecret.Type,
                    Value = updateSecretDto.NewValue,
                    updateSecretDto.Description,
                    updateSecretDto.Expiration
                });
        }

        [Theory(DisplayName = "Fail when updating a client secret and the client does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test054(UpdateClientSecretDto clientSecretUpdate, string clientId)
        {
            using var response = await Client.UpdateClientSecret(clientId, clientSecretUpdate);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Fail when updating a client secret that does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test055(ClientCreateDto existingClient, UpdateClientSecretDto clientSecretUpdate, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var response = await Client.UpdateClientSecret(clientId, clientSecretUpdate);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Pass when deleting a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test056(ClientCreateDto existingClient, CreateClientSecretDto clientSecret, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, clientSecret);

            using var response = await Client.DeleteClientSecret(clientId, clientSecret.Id);

            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Fail when deleting a client secret that does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test057(ClientCreateDto existingClient, string clientId, int secretId)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var response = await Client.DeleteClientSecret(clientId, secretId);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Fail when deleting a client secret and the client does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test058(string clientId, int secretId)
        {
            using var response = await Client.DeleteClientSecret(clientId, secretId);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Pass when getting a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test059(ClientCreateDto existingClient, CreateClientSecretDto clientSecret, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, clientSecret);

            var actualClientSecret = await Client.GetClientSecret(clientId, clientSecret.Id);

            actualClientSecret.Should().NotBeNull().And
                .BeEquivalentTo(new
                {
                    clientSecret.Id,
                    clientSecret.Type,
                    clientSecret.Value,
                    clientSecret.Expiration
                });
        }

        [Theory(DisplayName = "Fail when getting a client secret that does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test060(ClientCreateDto existingClient, CreateClientSecretDto clientSecret, string clientId)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            Func<Task> f = () => Client.GetClientSecret(clientId, clientSecret.Id);

            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory(DisplayName = "Pass when getting a client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test6(ClientCreateDto client, string clientId)
        {
            using var _ = await Client.CreateClient(clientId, client);

            var actualClient = await Client.GetClient(clientId);

            actualClient.Should().BeEquivalentTo(client);
            actualClient!.ClientId.Should().Be(clientId);
        }

        [Theory(DisplayName = "Fail when getting a client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test61(string clientId)
        {
            Func<Task> f = () => Client.GetClient(clientId);

            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}