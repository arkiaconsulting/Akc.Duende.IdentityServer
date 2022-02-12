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
    public class ApiScopeManagementPipelineTests
    {
        private HttpClient Client => _factory.CreateClient(new() { BaseAddress = new($"https://localhost{TestProgram.BasePath}/") });
        private readonly DefaultWebApplicationFactory _factory;

        public ApiScopeManagementPipelineTests() =>
            _factory = new DefaultWebApplicationFactory();

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Pass when creating an Api scope")]
        [InlineAutoData]
        public async Task Test02(string name, CreateUpdateApiScopeDto dto)
        {
            using var response = await Client.CreateApiScope(name, dto);

            response.Should().Be201Created();
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Fail when getting an Api scope that does not exist")]
        [InlineAutoData]
        public async Task Test03(string name)
        {
            Func<Task> f = () => Client.GetApiScope(name);

            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.BadRequest);
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Pass when getting an Api scope that does exist")]
        [InlineAutoData]
        public async Task Test04(string name, CreateUpdateApiScopeDto dto)
        {
            await Client.CreateApiScope(name, dto);

            var scope = await Client.GetApiScope(name);

            scope.Should().NotBeNull();
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Get right Api scope data when getting it")]
        [InlineAutoData]
        public async Task Test05(string name, CreateUpdateApiScopeDto dto)
        {
            await Client.CreateApiScope(name, dto);

            var scope = await Client.GetApiScope(name);

            scope!.Should().BeEquivalentTo(new ApiScopeDto(
                name,
                dto.DisplayName,
                dto.Description,
                dto.ShowInDiscoveryDocument,
                dto.UserClaims,
                dto.Properties,
                dto.Enabled,
                dto.Required
            ));
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Fail when creating an Api scope that already exists")]
        [InlineAutoData]
        public async Task Test06(string name, CreateUpdateApiScopeDto dto)
        {
            await Client.CreateApiScope(name, dto);

            using var response = await Client.CreateApiScope(name, dto);

            response.Should().Be400BadRequest();
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Fail when updating an Api scope that does not exist")]
        [InlineAutoData]
        public async Task Test07(string name, CreateUpdateApiScopeDto dto)
        {
            using var response = await Client.UpdateApiScope(name, dto);

            response.Should().Be400BadRequest();
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Pass when updating an Api scope that does already exist")]
        [InlineAutoData]
        public async Task Test08(string name, CreateUpdateApiScopeDto dto)
        {
            await Client.CreateApiScope(name, dto);

            using var response = await Client.UpdateApiScope(name, dto);

            response.Should().Be200Ok();
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Effectively update the Api scope")]
        [InlineAutoData]
        public async Task Test09(string name, CreateUpdateApiScopeDto dto, CreateUpdateApiScopeDto updatedDto)
        {
            await Client.CreateApiScope(name, dto);

            await Client.UpdateApiScope(name, updatedDto);

            var actualScope = await Client.GetApiScope(name);
            actualScope!.Should().BeEquivalentTo(new ApiScopeDto(
                name,
                updatedDto.DisplayName,
                updatedDto.Description,
                updatedDto.ShowInDiscoveryDocument,
                updatedDto.UserClaims,
                updatedDto.Properties,
                updatedDto.Enabled,
                updatedDto.Required
            ));
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Fail when deleting an Api scope that does not exist")]
        [InlineAutoData]
        public async Task Test10(string name)
        {
            using var response = await Client.DeleteApiScope(name);

            response.Should().Be404NotFound();
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Pass when deleting an Api scope that does already exist")]
        [InlineAutoData]
        public async Task Test11(string name, CreateUpdateApiScopeDto dto)
        {
            await Client.CreateApiScope(name, dto);

            using var response = await Client.DeleteApiScope(name);

            response.Should().Be200Ok();
        }

        [Trait("Category", "API_SCOPE")]
        [Theory(DisplayName = "Effectively delete an Api scope")]
        [InlineAutoData]
        public async Task Test12(string name, CreateUpdateApiScopeDto dto)
        {
            await Client.CreateApiScope(name, dto);

            await Client.DeleteApiScope(name);

            Func<Task> f = () => Client.GetApiScope(name);
            await f.Should().ThrowAsync<HttpRequestException>()
                .Where(e => e.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}