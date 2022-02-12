// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Akc.Duende.IdentityServer.Management.Api.Tests.Assets
{
    internal static class HttpHelpers
    {
        public static Task<HttpResponseMessage> CreateClient(this HttpClient client, string clientId, ClientCreateDto dto) =>
            client.PutAsJsonAsync($"{clientId}", dto);

        public static Task<ClientDto[]?> GetClients(this HttpClient client) =>
            client.GetFromJsonAsync<ClientDto[]>(string.Empty);

        public static Task<ClientDto?> GetClient(this HttpClient client, string clientId) =>
            client.GetFromJsonAsync<ClientDto>($"{clientId}");

        public static Task<HttpResponseMessage> UpdateClient(this HttpClient client, string clientId, ClientUpdateDto dto) =>
            client.PostAsJsonAsync($"{clientId}", dto);

        public static Task<HttpResponseMessage> DeleteClient(this HttpClient client, string clientId) =>
            client.DeleteAsync($"{clientId}");

        public static Task<HttpResponseMessage> CreateClientSecret(this HttpClient client, string clientId, CreateClientSecretDto dto) =>
            client.PutAsJsonAsync($"{clientId}/secrets", dto);

        public static Task<HttpResponseMessage> UpdateClientSecret(this HttpClient client, string clientId, UpdateClientSecretDto dto) =>
            client.PostAsJsonAsync($"{clientId}/secrets", dto);

        public static Task<HttpResponseMessage> DeleteClientSecret(this HttpClient client, string clientId, int id) =>
            client.DeleteAsync($"{clientId}/secrets/{id}");

        public static Task<SecretDto?> GetClientSecret(this HttpClient client, string clientId, int id) =>
            client.GetFromJsonAsync<SecretDto>($"{clientId}/secrets/{id}");

        public static Task<ApiScopeDto?> GetApiScopes(this HttpClient client) =>
            client.GetFromJsonAsync<ApiScopeDto>(string.Empty);
    }
}
