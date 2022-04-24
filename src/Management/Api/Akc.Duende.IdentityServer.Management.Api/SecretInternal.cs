// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class SecretInternal : Secret
    {
        public string Id { get; }

        public SecretInternal(string id, string type, string value, string description, DateTime? expiration)
            : base(value, description, expiration)
        {
            Id = id;
            Type = type;
        }

        public static SecretInternal FromModel(Secret secret, string id) =>
            new(id, secret.Type, secret.Value, secret.Description, secret.Expiration);
    }
}