// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api.Tests.Assets;
using AutoFixture.Xunit2;
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
    public class ClientSecretManagementPipelineTests
    {
        private HttpClient Client { get; }
        private DefaultTestData TestData => _factory.Services.GetRequiredService<DefaultTestData>();
        private readonly DefaultWebApplicationFactory _factory;

        public ClientSecretManagementPipelineTests()
        {
            _factory = new DefaultWebApplicationFactory();
            Client = _factory.CreateClient(new() { BaseAddress = new($"https://localhost{TestProgram.BasePath}/") });
        }

        [Theory(DisplayName = "Add a new client secret to an existing client")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test01(ClientCreateDto existingClient, CreateClientSecretDto newClientSecret, string clientId, string clientSecretName)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var response = await Client.CreateClientSecret(clientId, clientSecretName, newClientSecret);

            response.Should().Be201Created();
            var actualClientSecret = await Client.GetClientSecret(clientId, clientSecretName);
            actualClientSecret.Should().NotBeNull().And
                .BeEquivalentTo(new
                {
                    Name = clientSecretName,
                    newClientSecret.Type,
                    Value = newClientSecret.Value.Sha256(),
                    newClientSecret.Description,
                    newClientSecret.Expiration
                });
        }

        [Theory(DisplayName = "Fail adding a client secret when the client does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test02(string clientId, string clientSecretName, CreateClientSecretDto newClientSecret)
        {
            using var response = await Client.CreateClientSecret(clientId, clientSecretName, newClientSecret);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Pass when adding a client secret that already exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test03(ClientCreateDto existingClient, CreateClientSecretDto newClientSecret, string clientId, string clientSecretName)
        {
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, clientSecretName, newClientSecret);

            using var response = await Client.CreateClientSecret(clientId, clientSecretName, newClientSecret);

            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Pass when updating a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test04(ClientCreateDto existingClient, CreateClientSecretDto initialClientSecret, string clientId, string clientSecretName, string newSecret, string description, DateTime newExpiry)
        {
            var updateSecretDto = new UpdateClientSecretDto(newSecret, description, newExpiry);
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, clientSecretName, initialClientSecret);

            using var response = await Client.UpdateClientSecret(clientId, clientSecretName, updateSecretDto);

            response.Should().Be200Ok();
            var actualClientSecret = await Client.GetClientSecret(clientId, clientSecretName);
            actualClientSecret.Should().NotBeNull().And
                .BeEquivalentTo(new
                {
                    Name = clientSecretName,
                    initialClientSecret.Type,
                    Value = updateSecretDto.NewValue,
                    updateSecretDto.Description,
                    updateSecretDto.Expiration
                });
        }

        [Theory(DisplayName = "Fail when updating a client secret and the client does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test05(UpdateClientSecretDto clientSecretUpdate, string clientId, string clientSecretName)
        {
            using var response = await Client.UpdateClientSecret(clientId, clientSecretName, clientSecretUpdate);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Fail when updating a client secret that does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test06(ClientCreateDto existingClient, UpdateClientSecretDto clientSecretUpdate, string clientId, string clientSecretName)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var response = await Client.UpdateClientSecret(clientId, clientSecretName, clientSecretUpdate);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Pass when deleting a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test07(ClientCreateDto existingClient, CreateClientSecretDto clientSecret, string clientId, string clientSecretName)
        {
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, clientSecretName, clientSecret);

            using var response = await Client.DeleteClientSecret(clientId, clientSecretName);

            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Fail when deleting a client secret that does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test08(ClientCreateDto existingClient, string clientId, string clientSecretName)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            using var response = await Client.DeleteClientSecret(clientId, clientSecretName);

            response.Should().Be404NotFound();
        }

        [Theory(DisplayName = "Fail when deleting a client secret and the client does not exists")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test09(string clientId, string clientSecretName)
        {
            using var response = await Client.DeleteClientSecret(clientId, clientSecretName);

            response.Should().Be400BadRequest();
        }

        [Theory(DisplayName = "Pass when getting a client secret")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test010(ClientCreateDto existingClient, CreateClientSecretDto clientSecret, string clientId, string clientSecretName)
        {
            _ = await Client.CreateClient(clientId, existingClient);
            _ = await Client.CreateClientSecret(clientId, clientSecretName, clientSecret);

            var actualClientSecret = await Client.GetClientSecret(clientId, clientSecretName);

            actualClientSecret.Should().NotBeNull().And
                .BeEquivalentTo(new
                {
                    Name = clientSecretName,
                    clientSecret.Type,
                    Value = clientSecret.Value.Sha256(),
                    clientSecret.Expiration
                });
        }

        [Theory(DisplayName = "Fail when getting a client secret that does not exist")]
        [Trait("Category", "CLIENT_SECRET")]
        [InlineAutoData]
        public async Task Test011(ClientCreateDto existingClient, string clientId, string clientSecretName)
        {
            _ = await Client.CreateClient(clientId, existingClient);

            Func<Task> f = () => Client.GetClientSecret(clientId, clientSecretName);

            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.NotFound);
        }
    }
}