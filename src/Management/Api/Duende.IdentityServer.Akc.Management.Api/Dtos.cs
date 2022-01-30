﻿// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    public record ClientInputDto(
        string ClientName,
        string Description,
        bool Enabled,
        string ProtocolType,
        bool RequireClientSecret,
        string ClientUri,
        string LogoUri,
        bool RequireConsent,
        bool AllowRememberConsent,
        bool RequirePkce,
        bool AllowPlainTextPkce,
        string[] AllowedGrantTypes,
        string[] RedirectUris,
        string[] PostLogoutRedirectUris,
        bool RequireRequestObject,
        bool AllowAccessTokensViaBrowser,
        string FrontChannelLogoutUri,
        string BackChannelLogoutUri,
        bool FrontChannelLogoutSessionRequired,
        bool BackChannelLogoutSessionRequired,
        bool AllowOfflineAccess,
        string[] AllowedScopes,
        bool AlwaysIncludeUserClaimsInIdToken,
        int IdentityTokenLifetime,
        int AuthorizationCodeLifetime,
        string[] AllowedIdentityTokenSigningAlgorithms,
        int AbsoluteRefreshTokenLifetime,
        int AccessTokenLifetime,
        int SlidingRefreshTokenLifetime,
        int? ConsentLifetime,
        TokenUsage RefreshTokenUsage,
        bool UpdateAccessTokenClaimsOnRefresh,
        TokenExpiration RefreshTokenExpiration,
        AccessTokenType AccessTokenType,
        bool EnableLocalLogin,
        string[] IdentityProviderRestrictions,
        bool IncludeJwtId,
        ClientClaim[] Claims,
        bool AlwaysSendClientClaims,
        string ClientClaimsPrefix,
        string PairWiseSubjectSalt,
        int? UserSsoLifetime,
        string UserCodeType,
        int DeviceCodeLifetime,
        int? CibaLifetime,
        int? PollingInterval,
        string[] AllowedCorsOrigins,
        IDictionary<string, string> Properties);

    public record ClientOutputDto(
        string ClientId,
        string ClientName,
        string Description,
        bool Enabled,
        string ProtocolType,
        bool RequireClientSecret,
        string ClientUri,
        string LogoUri,
        bool RequireConsent,
        bool AllowRememberConsent,
        bool RequirePkce,
        bool AllowPlainTextPkce,
        string[] AllowedGrantTypes,
        string[] RedirectUris,
        string[] PostLogoutRedirectUris,
        bool RequireRequestObject,
        bool AllowAccessTokensViaBrowser,
        string FrontChannelLogoutUri,
        string BackChannelLogoutUri,
        bool FrontChannelLogoutSessionRequired,
        bool BackChannelLogoutSessionRequired,
        bool AllowOfflineAccess,
        string[] AllowedScopes,
        bool AlwaysIncludeUserClaimsInIdToken,
        int IdentityTokenLifetime,
        int AuthorizationCodeLifetime,
        string[] AllowedIdentityTokenSigningAlgorithms,
        int AbsoluteRefreshTokenLifetime,
        int AccessTokenLifetime,
        int SlidingRefreshTokenLifetime,
        int? ConsentLifetime,
        TokenUsage RefreshTokenUsage,
        bool UpdateAccessTokenClaimsOnRefresh,
        TokenExpiration RefreshTokenExpiration,
        AccessTokenType AccessTokenType,
        bool EnableLocalLogin,
        string[] IdentityProviderRestrictions,
        bool IncludeJwtId,
        ClientClaim[] Claims,
        bool AlwaysSendClientClaims,
        string ClientClaimsPrefix,
        string PairWiseSubjectSalt,
        int? UserSsoLifetime,
        string UserCodeType,
        int DeviceCodeLifetime,
        int? CibaLifetime,
        int? PollingInterval,
        string[] AllowedCorsOrigins,
        IDictionary<string, string> Properties);

    public record CreateClientSecretInputDto(string Type, string Value, DateTime? Expiration);

    public record UpdateClientSecretInputDto(string Type, string Value, string NewValue, DateTime? Expiration);
}
