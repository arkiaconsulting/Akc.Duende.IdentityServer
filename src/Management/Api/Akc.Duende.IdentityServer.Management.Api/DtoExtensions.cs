// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;
using IdentityModel;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal static class DtoExtensions
    {
        public static Client ToModel(this ClientInputDto dto, string clientId)
        {
            var client = new Client
            {
                ClientId = clientId,
                ClientName = dto.ClientName,
                Description = dto.Description,
                AllowedCorsOrigins = dto.AllowedCorsOrigins,
                AllowedGrantTypes = dto.AllowedGrantTypes,
                AllowedIdentityTokenSigningAlgorithms = dto.AllowedIdentityTokenSigningAlgorithms,
                AllowedScopes = dto.AllowedScopes,
                BackChannelLogoutUri = dto.BackChannelLogoutUri,
                CibaLifetime = dto.CibaLifetime,
                Claims = dto.Claims,
                ClientUri = dto.ClientUri,
                ConsentLifetime = dto.ConsentLifetime,
                FrontChannelLogoutUri = dto.FrontChannelLogoutUri,
                IdentityProviderRestrictions = dto.IdentityProviderRestrictions,
                LogoUri = dto.LogoUri,
                PairWiseSubjectSalt = dto.PairWiseSubjectSalt,
                PollingInterval = dto.PollingInterval,
                PostLogoutRedirectUris = dto.PostLogoutRedirectUris,
                Properties = dto.Properties,
                RedirectUris = dto.RedirectUris,
                UserCodeType = dto.UserCodeType,
                UserSsoLifetime = dto.UserSsoLifetime
            };

            if (!string.IsNullOrWhiteSpace(dto.ProtocolType))
            {
                client.ProtocolType = dto.ProtocolType;
            }

            if (!string.IsNullOrWhiteSpace(dto.ClientClaimsPrefix))
            {
                client.ClientClaimsPrefix = dto.ClientClaimsPrefix;
            }

            if (dto.AbsoluteRefreshTokenLifetime.HasValue)
            {
                client.AbsoluteRefreshTokenLifetime = dto.AbsoluteRefreshTokenLifetime.Value;
            }

            if (dto.AccessTokenLifetime.HasValue)
            {
                client.AccessTokenLifetime = dto.AccessTokenLifetime.Value;
            }

            if (dto.AccessTokenType.HasValue)
            {
                client.AccessTokenType = dto.AccessTokenType.Value;
            }

            if (dto.AllowAccessTokensViaBrowser.HasValue)
            {
                client.AllowAccessTokensViaBrowser = dto.AllowAccessTokensViaBrowser.Value;
            }

            if (dto.AllowOfflineAccess.HasValue)
            {
                client.AllowOfflineAccess = dto.AllowOfflineAccess.Value;
            }

            if (dto.AllowPlainTextPkce.HasValue)
            {
                client.AllowPlainTextPkce = dto.AllowPlainTextPkce.Value;
            }

            if (dto.AllowRememberConsent.HasValue)
            {
                client.AllowRememberConsent = dto.AllowRememberConsent.Value;
            }

            if (dto.AlwaysIncludeUserClaimsInIdToken.HasValue)
            {
                client.AlwaysIncludeUserClaimsInIdToken = dto.AlwaysIncludeUserClaimsInIdToken.Value;
            }

            if (dto.AlwaysSendClientClaims.HasValue)
            {
                client.AlwaysSendClientClaims = dto.AlwaysSendClientClaims.Value;
            }

            if (dto.AuthorizationCodeLifetime.HasValue)
            {
                client.AuthorizationCodeLifetime = dto.AuthorizationCodeLifetime.Value;
            }

            if (dto.BackChannelLogoutSessionRequired.HasValue)
            {
                client.BackChannelLogoutSessionRequired = dto.BackChannelLogoutSessionRequired.Value;
            }

            if (dto.DeviceCodeLifetime.HasValue)
            {
                client.DeviceCodeLifetime = dto.DeviceCodeLifetime.Value;
            }

            if (dto.Enabled.HasValue)
            {
                client.Enabled = dto.Enabled.Value;
            }

            if (dto.EnableLocalLogin.HasValue)
            {
                client.EnableLocalLogin = dto.EnableLocalLogin.Value;
            }

            if (dto.FrontChannelLogoutSessionRequired.HasValue)
            {
                client.FrontChannelLogoutSessionRequired = dto.FrontChannelLogoutSessionRequired.Value;
            }

            if (dto.IdentityTokenLifetime.HasValue)
            {
                client.IdentityTokenLifetime = dto.IdentityTokenLifetime.Value;
            }

            if (dto.IncludeJwtId.HasValue)
            {
                client.IncludeJwtId = dto.IncludeJwtId.Value;
            }

            if (dto.RefreshTokenExpiration.HasValue)
            {
                client.RefreshTokenExpiration = dto.RefreshTokenExpiration.Value;
            }

            if (dto.RefreshTokenUsage.HasValue)
            {
                client.RefreshTokenUsage = dto.RefreshTokenUsage.Value;
            }

            if (dto.RequireClientSecret.HasValue)
            {
                client.RequireClientSecret = dto.RequireClientSecret.Value;
            }

            if (dto.RequireConsent.HasValue)
            {
                client.RequireConsent = dto.RequireConsent.Value;
            }

            if (dto.RequirePkce.HasValue)
            {
                client.RequirePkce = dto.RequirePkce.Value;
            }

            if (dto.RequireRequestObject.HasValue)
            {
                client.RequireRequestObject = dto.RequireRequestObject.Value;
            }

            if (dto.SlidingRefreshTokenLifetime.HasValue)
            {
                client.SlidingRefreshTokenLifetime = dto.SlidingRefreshTokenLifetime.Value;
            }

            if (dto.UpdateAccessTokenClaimsOnRefresh.HasValue)
            {
                client.UpdateAccessTokenClaimsOnRefresh = dto.UpdateAccessTokenClaimsOnRefresh.Value;
            }

            return client;
        }

        public static ClientOutputDto FromModel(this Client model) =>
            new(model.ClientId)
            {
                ClientName = model.ClientName,
                Description = model.Description,
                Enabled = model.Enabled,
                ProtocolType = model.ProtocolType,
                RequireClientSecret = model.RequireClientSecret,
                ClientUri = model.ClientUri,
                LogoUri = model.LogoUri,
                RequireConsent = model.RequireConsent,
                AllowRememberConsent = model.AllowRememberConsent,
                RequirePkce = model.RequirePkce,
                AllowPlainTextPkce = model.AllowPlainTextPkce,
                AllowedGrantTypes = model.AllowedGrantTypes?.ToArray(),
                RedirectUris = model.RedirectUris?.ToArray() ?? Array.Empty<string>(),
                PostLogoutRedirectUris = model.PostLogoutRedirectUris?.ToArray() ?? Array.Empty<string>(),
                RequireRequestObject = model.RequireRequestObject,
                AllowAccessTokensViaBrowser = model.AllowAccessTokensViaBrowser,
                FrontChannelLogoutUri = model.FrontChannelLogoutUri,
                BackChannelLogoutUri = model.BackChannelLogoutUri,
                FrontChannelLogoutSessionRequired = model.FrontChannelLogoutSessionRequired,
                BackChannelLogoutSessionRequired = model.BackChannelLogoutSessionRequired,
                AllowOfflineAccess = model.AllowOfflineAccess,
                AllowedScopes = model.AllowedScopes?.ToArray(),
                AlwaysIncludeUserClaimsInIdToken = model.AlwaysIncludeUserClaimsInIdToken,
                IdentityTokenLifetime = model.IdentityTokenLifetime,
                AuthorizationCodeLifetime = model.AuthorizationCodeLifetime,
                AllowedIdentityTokenSigningAlgorithms = model.AllowedIdentityTokenSigningAlgorithms?.ToArray() ?? Array.Empty<string>(),
                AbsoluteRefreshTokenLifetime = model.AbsoluteRefreshTokenLifetime,
                AccessTokenLifetime = model.AccessTokenLifetime,
                SlidingRefreshTokenLifetime = model.SlidingRefreshTokenLifetime,
                ConsentLifetime = model.ConsentLifetime,
                RefreshTokenUsage = model.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh = model.UpdateAccessTokenClaimsOnRefresh,
                RefreshTokenExpiration = model.RefreshTokenExpiration,
                AccessTokenType = model.AccessTokenType,
                EnableLocalLogin = model.EnableLocalLogin,
                IdentityProviderRestrictions = model.IdentityProviderRestrictions?.ToArray() ?? Array.Empty<string>(),
                IncludeJwtId = model.IncludeJwtId,
                Claims = model.Claims?.ToArray() ?? Array.Empty<ClientClaim>(),
                AlwaysSendClientClaims = model.AlwaysSendClientClaims,
                ClientClaimsPrefix = model.ClientClaimsPrefix,
                PairWiseSubjectSalt = model.PairWiseSubjectSalt,
                UserSsoLifetime = model.UserSsoLifetime,
                UserCodeType = model.UserCodeType,
                DeviceCodeLifetime = model.DeviceCodeLifetime,
                CibaLifetime = model.CibaLifetime,
                PollingInterval = model.PollingInterval,
                AllowedCorsOrigins = model.AllowedCorsOrigins?.ToArray() ?? Array.Empty<string>(),
                Properties = model.Properties ?? new Dictionary<string, string>()
            };

        public static Secret ToModel(this CreateClientSecretInputDto dto) =>
            new(dto.Value.ToSha256(), dto.Description, dto.Expiration) { Type = dto.Type };

        public static SecretDto ToDto(this Secret model, int id) =>
            new(id, model.Type, model.Value, model.Description, model.Expiration);

        public static ApiScopeDto ToDto(this ApiScope model) =>
            new(model.Name,
                model.DisplayName,
                model.Description,
                model.ShowInDiscoveryDocument,
                model.UserClaims.ToArray(),
                model.Properties,
                model.Enabled,
                model.Required,
                model.Emphasize);

        public static ApiScope ToModel(this CreateUpdateApiScopeDto dto, string name) =>
            new(name,
                dto.DisplayName,
                dto.UserClaims)
            {
                Description = dto.Description,
                Enabled = dto.Enabled,
                Properties = dto.Properties,
                Required = dto.Required,
                ShowInDiscoveryDocument = dto.ShowInDiscoveryDocument,
                Emphasize = dto.Emphasize
            };

        public static ApiResourceDto ToDto(this ApiResource model) =>
            new(model.Name,
                model.DisplayName,
                model.Scopes.ToArray());

        public static ApiResource ToModel(this CreateUpdateApiResourceDto dto, string name) =>
            new(name,
                dto.DisplayName,
                dto.Scopes);
    }
}
