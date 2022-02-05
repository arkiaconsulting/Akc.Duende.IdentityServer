// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Akc.Duende.IdentityServer.Management.Api.Tests.Assets
{
    internal class BearerBypass : AuthenticationHandler<NullOptions>
    {
        public BearerBypass(IOptionsMonitor<NullOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()) };
            var identity = new ClaimsIdentity(claims, IdentityServerConstants.LocalApi.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, IdentityServerConstants.LocalApi.AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

    public class NullOptions : AuthenticationSchemeOptions { }

    public static class TestAuthorizationExtensions
    {
        public static IServiceCollection AddIdentityServerLocalJwtBearerByPass(this IServiceCollection services) =>
            services.AddAuthenticationCore(options =>
            {
                var authBuilder = options.SchemeMap[IdentityServerConstants.LocalApi.AuthenticationScheme]!;
                authBuilder.HandlerType = typeof(BearerBypass);
            });
    }
}
