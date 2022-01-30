// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api.Tests.Assets;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Akc.Duende.IdentityServer.Management.Api.Tests
{
    [Trait("Category", "Integration")]
    public class ClientManagementPipelineTests
    {
        private HttpClient Client { get; }
        private string AnySecretType { get; } = "AnySecretType";
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
            (await GetClients()).Should().BeEquivalentTo(
                TestData.Clients,
                opts => opts.Excluding(c => c.ClientSecrets));

        [Theory(DisplayName = "Add a new client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test02(ClientCreateDto client, string clientId)
        {
            using var _ = await CreateClient(clientId, client);

            (await GetClients()).Should().ContainEquivalentOf(client).Subject
                .ClientId.Should().Be(clientId.ToString());
        }

        [Theory(DisplayName = "Update an existing client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test03(ClientCreateDto existingClient, ClientUpdateDto updatedClient, string clientId)
        {
            _ = await CreateClient(clientId, existingClient);

            using var _1 = await UpdateClient(clientId, updatedClient);

            (await GetClients()).Should().ContainEquivalentOf(updatedClient).Subject
                .ClientId.Should().Be(clientId.ToString());
        }

        [Theory(DisplayName = "Update a client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test031(ClientUpdateDto updatedClient, string clientId)
        {
            using var response = await UpdateClient(clientId, updatedClient);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Delete an existing client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test04(ClientCreateDto existingClient, string clientId)
        {
            _ = await CreateClient(clientId, existingClient);

            using var _1 = await DeleteClient(clientId);

            (await GetClients()).Should().NotContainEquivalentOf(existingClient);
        }

        [Theory(DisplayName = "Delete an existing client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test041(string clientId)
        {
            using var response = await DeleteClient(clientId);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Add a new client secret to an existing client")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test05(ClientCreateDto existingClient, string clientId, string secret, DateTime secretExpiry)
        {
            var secretDto = new CreateClientSecretDto(AnySecretType, secret, secretExpiry);
            _ = await CreateClient(clientId, existingClient);

            using var response = await CreateClientSecret(clientId, secretDto);

            response.Should().Be201Created();
        }

        [Theory(DisplayName = "Fail adding a client secret when the client does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test051(string clientId, string secret, DateTime secretExpiry)
        {
            var secretDto = new CreateClientSecretDto(AnySecretType, secret, secretExpiry);

            using var response = await CreateClientSecret(clientId, secretDto);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Pass when adding a client secret that already exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test052(ClientCreateDto existingClient, string clientId, string secret, DateTime secretExpiry)
        {
            var secretDto = new CreateClientSecretDto(AnySecretType, secret, secretExpiry);
            _ = await CreateClient(clientId, existingClient);
            _ = await CreateClientSecret(clientId, secretDto);

            using var response = await CreateClientSecret(clientId, secretDto);

            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Pass when updating a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test053(ClientCreateDto existingClient, string clientId, string secret, string newSecret, DateTime newExpiry)
        {
            var initialSecretDto = new CreateClientSecretDto(AnySecretType, secret, default);
            var updateSecretDto = new UpdateClientSecretDto(AnySecretType, secret, newSecret, newExpiry);
            _ = await CreateClient(clientId, existingClient);
            _ = await CreateClientSecret(clientId, initialSecretDto);

            using var response = await UpdateClientSecret(clientId, updateSecretDto);

            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Fail when updating a client secret and the client does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test054(string clientId, string secret, string newSecret, DateTime newExpiry)
        {
            var updateSecretDto = new UpdateClientSecretDto(AnySecretType, secret, newSecret, newExpiry);

            using var response = await UpdateClientSecret(clientId, updateSecretDto);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Fail when updating a client secret that does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test055(ClientCreateDto existingClient, string clientId, string secret, string newSecret, DateTime newExpiry)
        {
            var updateSecretDto = new UpdateClientSecretDto(AnySecretType, secret, newSecret, newExpiry);
            _ = await CreateClient(clientId, existingClient);

            using var response = await UpdateClientSecret(clientId, updateSecretDto);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Pass when deleting a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test056(ClientCreateDto existingClient, string clientId, string secret)
        {
            var secretDto = new CreateClientSecretDto(AnySecretType, secret, default);
            _ = await CreateClient(clientId, existingClient);
            _ = await CreateClientSecret(clientId, secretDto);

            using var response = await DeleteClientSecret(clientId, AnySecretType, secret);

            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Fail when deleting a client secret that does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test057(ClientCreateDto existingClient, string clientId, string secret)
        {
            _ = await CreateClient(clientId, existingClient);

            using var response = await DeleteClientSecret(clientId, AnySecretType, secret);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Fail when deleting a client secret and the client does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test058(string clientId, string secret)
        {
            using var response = await DeleteClientSecret(clientId, AnySecretType, secret);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Pass when getting a client")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test6(ClientCreateDto client, string clientId)
        {
            using var _ = await CreateClient(clientId, client);

            var actualClient = await GetClient(clientId);

            actualClient.Should().BeEquivalentTo(client);
            actualClient!.ClientId.Should().Be(clientId);
        }

        [Theory(DisplayName = "Fail when getting a client that does not exist")]
        [Trait("Category", "CLIENT")]
        [InlineAutoData]
        public async Task Test61(string clientId)
        {
            Func<Task> f = () => GetClient(clientId);

            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.BadRequest);
        }

        #region Private

        private Task<HttpResponseMessage> CreateClient(string clientId, ClientCreateDto dto) =>
            Client.PutAsJsonAsync($"{clientId}", dto);

        private Task<ClientDto[]?> GetClients() =>
            Client.GetFromJsonAsync<ClientDto[]>(string.Empty);

        private Task<ClientDto?> GetClient(string clientId) =>
            Client.GetFromJsonAsync<ClientDto>($"{clientId}");

        private Task<HttpResponseMessage> UpdateClient(string clientId, ClientUpdateDto dto) =>
            Client.PostAsJsonAsync($"{clientId}", dto);

        private Task<HttpResponseMessage> DeleteClient(string clientId) =>
            Client.DeleteAsync($"{clientId}");

        private Task<HttpResponseMessage> CreateClientSecret(string clientId, CreateClientSecretDto dto) =>
            Client.PutAsJsonAsync($"{clientId}/secrets", dto);

        private Task<HttpResponseMessage> UpdateClientSecret(string clientId, UpdateClientSecretDto dto) =>
            Client.PostAsJsonAsync($"{clientId}/secrets", dto);

        private Task<HttpResponseMessage> DeleteClientSecret(string clientId, string type, string secret) =>
            Client.DeleteAsync($"{clientId}/secrets/{type}/{secret}");

        #endregion
    }
}