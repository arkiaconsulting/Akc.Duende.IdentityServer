// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class SecretInternal
    {
        public int Id { get; }
        public string ClientId { get; }
        public string? Description { get; }
        public string Type { get; }
        public string Value { get; }
        public DateTime? Expiration { get; }

        public SecretInternal(int id, string clientId, string type, string value, DateTime? expiration, string description)
        {
            Id = id;
            ClientId = clientId;
            Type = type;
            Value = value;
            Expiration = expiration;
            Description = description;
        }

        public static SecretInternal FromModel(string clientId, Secret secret, int index) =>
            new(index, clientId, secret.Type, secret.Value, secret.Expiration, secret.Description);
    }
}