// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

namespace Duende.IdentityServer.Akc.Admin.Api
{
    internal static class DtoExtensions
    {
        public static Client ToModel(this ClientInputDto dto, string clientId) =>
            new()
            {
                ClientId = clientId,
                ClientName = dto.ClientName,
                Description = dto.Description,
                AbsoluteRefreshTokenLifetime = dto.AbsoluteRefreshTokenLifetime,
                AccessTokenLifetime = dto.AccessTokenLifetime,
                AccessTokenType = dto.AccessTokenType,
                AllowAccessTokensViaBrowser = dto.AllowAccessTokensViaBrowser,
                AllowedCorsOrigins = dto.AllowedCorsOrigins,
                AllowedGrantTypes = dto.AllowedGrantTypes,
                AllowedIdentityTokenSigningAlgorithms = dto.AllowedIdentityTokenSigningAlgorithms,
                AllowedScopes = dto.AllowedScopes,
                AllowOfflineAccess = dto.AllowOfflineAccess,
                AllowPlainTextPkce = dto.AllowPlainTextPkce,
                AllowRememberConsent = dto.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = dto.AlwaysIncludeUserClaimsInIdToken,
                AlwaysSendClientClaims = dto.AlwaysSendClientClaims,
                AuthorizationCodeLifetime = dto.AuthorizationCodeLifetime,
                BackChannelLogoutSessionRequired = dto.BackChannelLogoutSessionRequired,
                BackChannelLogoutUri = dto.BackChannelLogoutUri,
                CibaLifetime = dto.CibaLifetime,
                Claims = dto.Claims,
                ClientClaimsPrefix = dto.ClientClaimsPrefix,
                ClientUri = dto.ClientUri,
                ConsentLifetime = dto.ConsentLifetime,
                DeviceCodeLifetime = dto.DeviceCodeLifetime,
                Enabled = dto.Enabled,
                EnableLocalLogin = dto.EnableLocalLogin,
                FrontChannelLogoutSessionRequired = dto.FrontChannelLogoutSessionRequired,
                FrontChannelLogoutUri = dto.FrontChannelLogoutUri,
                IdentityProviderRestrictions = dto.IdentityProviderRestrictions,
                IdentityTokenLifetime = dto.IdentityTokenLifetime,
                IncludeJwtId = dto.IncludeJwtId,
                LogoUri = dto.LogoUri,
                PairWiseSubjectSalt = dto.PairWiseSubjectSalt,
                PollingInterval = dto.PollingInterval,
                PostLogoutRedirectUris = dto.PostLogoutRedirectUris,
                Properties = dto.Properties,
                ProtocolType = dto.ProtocolType,
                RedirectUris = dto.RedirectUris,
                RefreshTokenExpiration = dto.RefreshTokenExpiration,
                RefreshTokenUsage = dto.RefreshTokenUsage,
                RequireClientSecret = dto.RequireClientSecret,
                RequireConsent = dto.RequireConsent,
                RequirePkce = dto.RequirePkce,
                RequireRequestObject = dto.RequireRequestObject,
                SlidingRefreshTokenLifetime = dto.SlidingRefreshTokenLifetime,
                UpdateAccessTokenClaimsOnRefresh = dto.UpdateAccessTokenClaimsOnRefresh,
                UserCodeType = dto.UserCodeType,
                UserSsoLifetime = dto.UserSsoLifetime
            };
    }
}
