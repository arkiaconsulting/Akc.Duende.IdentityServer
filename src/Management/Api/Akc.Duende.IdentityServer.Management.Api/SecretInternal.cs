// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal class SecretInternal : Secret
    {
        public int Id { get; }

        public SecretInternal(int id, string type, string value, string description, DateTime? expiration)
            : base(value, description, expiration)
        {
            Id = id;
            Type = type;
        }

        public static SecretInternal FromModel(Secret secret, int index) =>
            new(index, secret.Type, secret.Value, secret.Description, secret.Expiration);
    }
}