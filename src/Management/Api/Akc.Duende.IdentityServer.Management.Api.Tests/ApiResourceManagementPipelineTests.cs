// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api.Tests.Assets;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Akc.Duende.IdentityServer.Management.Api.Tests
{
    [Trait("Category", "Integration")]
    public class ApiResourceManagementPipelineTests
    {
        private HttpClient Client => _factory.CreateClient(new() { BaseAddress = new($"https://localhost{TestProgram.BasePath}/") });
        private readonly DefaultWebApplicationFactory _factory;

        public ApiResourceManagementPipelineTests() =>
            _factory = new DefaultWebApplicationFactory();

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Pass when creating an Api resource")]
        [InlineAutoData]
        public async Task Test02(string name, CreateUpdateApiResourceDto dto)
        {
            using var response = await Client.CreateApiResource(name, dto);

            response.Should().Be201Created();
        }

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Fail when creating an Api resource that already exists")]
        [InlineAutoData]
        public async Task Test06(string name, CreateUpdateApiResourceDto dto)
        {
            await Client.CreateApiResource(name, dto);

            using var response = await Client.CreateApiResource(name, dto);

            response.Should().Be400BadRequest();
        }

    }
}