﻿// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;

namespace Akc.Duende.IdentityServer.Management.Api.Tests.Assets
{
    public record ClientDto(
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

    public record ClientCreateDto(
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

    public record ClientUpdateDto(
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

    public record CreateClientSecretDto(int Id, string Type, string Value, DateTime? Expiration);
    public record UpdateClientSecretDto(int Id, string Type, string Value, string NewValue, DateTime? Expiration);
}
