// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
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

        public static ClientOutputDto FromModel(this Client model) =>
            new(
                model.ClientId, model.ClientName, model.Description, model.Enabled, model.ProtocolType,
                model.RequireClientSecret, model.ClientUri, model.LogoUri, model.RequireConsent,
                model.AllowRememberConsent, model.RequirePkce,
                model.AllowPlainTextPkce, model.AllowedGrantTypes.ToArray(), model.RedirectUris.ToArray(),
                model.PostLogoutRedirectUris.ToArray(), model.RequireRequestObject, model.AllowAccessTokensViaBrowser,
                model.FrontChannelLogoutUri, model.BackChannelLogoutUri, model.FrontChannelLogoutSessionRequired,
                model.BackChannelLogoutSessionRequired, model.AllowOfflineAccess, model.AllowedScopes.ToArray(),
                model.AlwaysIncludeUserClaimsInIdToken, model.IdentityTokenLifetime, model.AuthorizationCodeLifetime,
                model.AllowedIdentityTokenSigningAlgorithms.ToArray(), model.AbsoluteRefreshTokenLifetime, model.AccessTokenLifetime,
                model.SlidingRefreshTokenLifetime, model.ConsentLifetime, model.RefreshTokenUsage, model.UpdateAccessTokenClaimsOnRefresh,
                model.RefreshTokenExpiration, model.AccessTokenType, model.EnableLocalLogin, model.IdentityProviderRestrictions.ToArray(),
                model.IncludeJwtId, model.Claims.ToArray(), model.AlwaysSendClientClaims, model.ClientClaimsPrefix,
                model.PairWiseSubjectSalt, model.UserSsoLifetime, model.UserCodeType, model.DeviceCodeLifetime,
                model.CibaLifetime, model.PollingInterval, model.AllowedCorsOrigins.ToArray(), model.Properties
            );

        public static Secret ToModel(this CreateClientSecretInputDto dto) =>
            new(dto.Value, dto.Description, dto.Expiration) { Type = dto.Type };

        public static SecretDto ToDto(this Secret model, int id) =>
            new(id, model.Type, model.Value, model.Description, model.Expiration);
    }
}
