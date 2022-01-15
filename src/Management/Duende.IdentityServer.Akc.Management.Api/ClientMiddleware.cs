// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal class ClientMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICollection<Client> _clients;

        public ClientMiddleware(RequestDelegate next, IEnumerable<Client> clients)
        {
            _next = next;
            if (clients is not ICollection<Client> clientStore)
            {
                throw new InvalidOperationException();
            }
            _clients = clientStore;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsGet())
            {
                await HandleGet(context);

                return;
            }
            else if (context.Request.IsPut())
            {
                await HandlePut(context);

                return;
            }
            else if (context.Request.IsPost())
            {
                await HandlePost(context);

                return;
            }

            await _next(context);
        }

        #region Private

        private async Task HandlePost(HttpContext context)
        {
            var dto = await Deserialize<ClientInputDto>(context.Request);
            if (dto == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return;
            }

            var clientId = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

            var existingClient = _clients.First(x => x.ClientId == clientId);
            _clients.Remove(existingClient);

            _clients.Add(dto.ToModel(clientId));
        }

        private async Task HandlePut(HttpContext context)
        {
            var dto = await Deserialize<ClientInputDto>(context.Request);
            if (dto == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return;
            }

            var clientId = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();

            _clients.Add(dto.ToModel(clientId));
        }

        private async Task HandleGet(HttpContext context)
        {
            var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

            var dto = _clients.Select(DtoExtensions.FromModel);

            await context.Response.WriteAsync(JsonSerializer.Serialize(dto, jsonOptions));
        }

        private static async Task<T?> Deserialize<T>(HttpRequest request)
        {
            var jsonOptions = request.HttpContext.RequestServices.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;

            return await JsonSerializer.DeserializeAsync<T>(request.Body, jsonOptions);
        }

        #endregion
    }
}