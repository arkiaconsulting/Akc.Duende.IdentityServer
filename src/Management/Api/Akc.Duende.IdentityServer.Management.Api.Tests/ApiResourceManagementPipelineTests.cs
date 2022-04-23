// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api.Tests.Assets;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Net;
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

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Fail when getting an Api resource that does not exist")]
        [InlineAutoData]
        public async Task Test03(string name)
        {
            Func<Task> f = () => Client.GetApiResource(name);

            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.NotFound);
        }

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Pass when getting an Api resource that does exist")]
        [InlineAutoData]
        public async Task Test04(string name, CreateUpdateApiResourceDto dto)
        {
            await Client.CreateApiResource(name, dto);

            var apiResource = await Client.GetApiResource(name);

            apiResource.Should().NotBeNull();
        }

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Get right Api resource data when getting it")]
        [InlineAutoData]
        public async Task Test05(string name, CreateUpdateApiResourceDto dto)
        {
            await Client.CreateApiResource(name, dto);

            var apiResource = await Client.GetApiResource(name);

            apiResource!.Should().BeEquivalentTo(new ApiResourceDto(
                name,
                dto.DisplayName,
                dto.Scopes
            ));
        }

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Fail when updating an Api resource that does not exist")]
        [InlineAutoData]
        public async Task Test07(string name, CreateUpdateApiResourceDto dto)
        {
            using var response = await Client.UpdateApiResource(name, dto);

            response.Should().Be400BadRequest();
        }

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Pass when updating an Api resource that does already exist")]
        [InlineAutoData]
        public async Task Test08(string name, CreateUpdateApiResourceDto dto)
        {
            await Client.CreateApiResource(name, dto);

            using var response = await Client.UpdateApiResource(name, dto);

            response.Should().Be200Ok();
        }

        [Trait("Category", "API_RESOURCE")]
        [Theory(DisplayName = "Effectively update the Api resource")]
        [InlineAutoData]
        public async Task Test09(string name, CreateUpdateApiResourceDto dto, CreateUpdateApiResourceDto updatedDto)
        {
            await Client.CreateApiResource(name, dto);

            await Client.UpdateApiResource(name, updatedDto);

            var actualApiResource = await Client.GetApiResource(name);
            actualApiResource!.Should().BeEquivalentTo(new ApiResourceDto(
                name,
                updatedDto.DisplayName,
                updatedDto.Scopes
            ));
        }
    }
}