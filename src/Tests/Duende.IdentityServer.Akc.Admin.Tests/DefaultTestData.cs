// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;

namespace Duende.IdentityServer.Akc.Admin.Tests
{
    internal static class DefaultTestData
    {
        public static readonly IEnumerable<Client> Clients = new List<Client>()
        {
            new Client { ClientId = Guid.NewGuid().ToString() }
        };

        public static readonly IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>()
        {
            new IdentityResources.OpenId()
        };
    }
}
