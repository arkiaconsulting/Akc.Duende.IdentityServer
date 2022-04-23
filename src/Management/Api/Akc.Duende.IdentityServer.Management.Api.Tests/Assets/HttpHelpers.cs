// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Akc.Duende.IdentityServer.Management.Api.Tests.Assets
{
    internal static class HttpHelpers
    {
        public static Task<HttpResponseMessage> CreateClient(this HttpClient client, string clientId, ClientCreateDto dto) =>
            client.PutAsJsonAsync($"clients/{clientId}", dto);

        public static Task<ClientDto[]?> GetClients(this HttpClient client) =>
            client.GetFromJsonAsync<ClientDto[]>("clients");

        public static Task<ClientDto?> GetClient(this HttpClient client, string clientId) =>
            client.GetFromJsonAsync<ClientDto>($"clients/{clientId}");

        public static Task<HttpResponseMessage> UpdateClient(this HttpClient client, string clientId, ClientUpdateDto dto) =>
            client.PostAsJsonAsync($"clients/{clientId}", dto);

        public static Task<HttpResponseMessage> DeleteClient(this HttpClient client, string clientId) =>
            client.DeleteAsync($"clients/{clientId}");

        public static Task<HttpResponseMessage> CreateClientSecret(this HttpClient client, string clientId, CreateClientSecretDto dto) =>
            client.PutAsJsonAsync($"clients/{clientId}/secrets", dto);

        public static Task<HttpResponseMessage> UpdateClientSecret(this HttpClient client, string clientId, UpdateClientSecretDto dto) =>
            client.PostAsJsonAsync($"clients/{clientId}/secrets", dto);

        public static Task<HttpResponseMessage> DeleteClientSecret(this HttpClient client, string clientId, int id) =>
            client.DeleteAsync($"clients/{clientId}/secrets/{id}");

        public static Task<SecretDto?> GetClientSecret(this HttpClient client, string clientId, int id) =>
            client.GetFromJsonAsync<SecretDto>($"clients/{clientId}/secrets/{id}");

        public static Task<ApiScopeDto?> GetApiScopes(this HttpClient client) =>
            client.GetFromJsonAsync<ApiScopeDto>("scopes");

        public static Task<ApiScopeDto?> GetApiScope(this HttpClient client, string name) =>
            client.GetFromJsonAsync<ApiScopeDto>($"scopes/{name}");

        public static Task<HttpResponseMessage> CreateApiScope(this HttpClient client, string name, CreateUpdateApiScopeDto dto) =>
            client.PutAsJsonAsync($"scopes/{name}", dto);

        public static Task<HttpResponseMessage> UpdateApiScope(this HttpClient client, string name, CreateUpdateApiScopeDto dto) =>
            client.PostAsJsonAsync($"scopes/{name}", dto);

        public static Task<HttpResponseMessage> DeleteApiScope(this HttpClient client, string name) =>
            client.DeleteAsync($"scopes/{name}");

        public static Task<HttpResponseMessage> CreateApiResource(this HttpClient client, string name, CreateUpdateApiResourceDto dto) =>
            client.PutAsJsonAsync($"resources/{name}", dto);

        public static Task<ApiResourceDto?> GetApiResource(this HttpClient client, string name) =>
            client.GetFromJsonAsync<ApiResourceDto>($"resources/{name}");

        public static Task<HttpResponseMessage> UpdateApiResource(this HttpClient client, string name, CreateUpdateApiResourceDto dto) =>
            client.PostAsJsonAsync($"resources/{name}", dto);
    }
}
