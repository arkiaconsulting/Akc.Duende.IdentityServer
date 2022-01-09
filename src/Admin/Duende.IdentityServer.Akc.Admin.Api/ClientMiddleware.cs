// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Duende.IdentityServer.Akc.Admin.Api
{
    internal class ClientMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<Client> _clients;

        public ClientMiddleware(RequestDelegate next, IEnumerable<Client> clients)
        {
            _next = next;
            _clients = clients;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethod.Get.Method)
            {
                var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;
                await context.Response.WriteAsync(JsonSerializer.Serialize(_clients, jsonOptions));

                return;
            }
            else if (context.Request.Method == HttpMethod.Put.Method)
            {
                if (_clients is not ICollection<Client> clientStore)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    return;
                }

                var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;
                var dto = await JsonSerializer.DeserializeAsync<ClientInputDto>(context.Request.Body, jsonOptions);
                if (dto == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    return;
                }

                var clientId = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

                clientStore.Add(dto.ToModel(clientId));

                return;
            }
            else if (context.Request.Method == HttpMethod.Post.Method)
            {
                if (_clients is not ICollection<Client> clientStore)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    return;
                }

                var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;
                var dto = await JsonSerializer.DeserializeAsync<ClientInputDto>(context.Request.Body, jsonOptions);
                if (dto == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    return;
                }

                var clientId = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

                var existingClient = clientStore.First(x => x.ClientId == clientId);
                clientStore.Remove(existingClient);

                clientStore.Add(dto.ToModel(clientId));

                return;
            }

            await _next(context);
        }
    }
}