// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer.Models;

namespace Akc.Duende.IdentityServer.Management.Api
{
    public class ClientInputDto
    {
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public bool? Enabled { get; set; }
        public string? ProtocolType { get; set; }
        public bool? RequireClientSecret { get; set; }
        public string? ClientUri { get; set; }
        public string? LogoUri { get; set; }
        public bool? RequireConsent { get; set; }
        public bool? AllowRememberConsent { get; set; }
        public bool? RequirePkce { get; set; }
        public bool? AllowPlainTextPkce { get; set; }
        public ICollection<string>? AllowedGrantTypes { get; set; }
        public string[]? RedirectUris { get; set; }
        public string[]? PostLogoutRedirectUris { get; set; }
        public bool? RequireRequestObject { get; set; }
        public bool? AllowAccessTokensViaBrowser { get; set; }
        public string? FrontChannelLogoutUri { get; set; }
        public string? BackChannelLogoutUri { get; set; }
        public bool? FrontChannelLogoutSessionRequired { get; set; }
        public bool? BackChannelLogoutSessionRequired { get; set; }
        public bool? AllowOfflineAccess { get; set; }
        public string[]? AllowedScopes { get; set; }
        public bool? AlwaysIncludeUserClaimsInIdToken { get; set; }
        public int? IdentityTokenLifetime { get; set; }
        public int? AuthorizationCodeLifetime { get; set; }
        public string[]? AllowedIdentityTokenSigningAlgorithms { get; set; }
        public int? AbsoluteRefreshTokenLifetime { get; set; }
        public int? AccessTokenLifetime { get; set; }
        public int? SlidingRefreshTokenLifetime { get; set; }
        public int? ConsentLifetime { get; set; }
        public TokenUsage? RefreshTokenUsage { get; set; }
        public bool? UpdateAccessTokenClaimsOnRefresh { get; set; }
        public TokenExpiration? RefreshTokenExpiration { get; set; }
        public AccessTokenType? AccessTokenType { get; set; }
        public bool? EnableLocalLogin { get; set; }
        public string[]? IdentityProviderRestrictions { get; set; }
        public bool? IncludeJwtId { get; set; }
        public ClientClaim[]? Claims { get; set; }
        public bool? AlwaysSendClientClaims { get; set; }
        public string? ClientClaimsPrefix { get; set; }
        public string? PairWiseSubjectSalt { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string? UserCodeType { get; set; }
        public int? DeviceCodeLifetime { get; set; }
        public int? CibaLifetime { get; set; }
        public int? PollingInterval { get; set; }
        public string[]? AllowedCorsOrigins { get; set; }
        public IDictionary<string, string>? Properties { get; set; }
    }

    public class ClientOutputDto
    {
        public ClientOutputDto(string clientId) => ClientId = clientId;

        public string ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public bool? Enabled { get; set; }
        public string? ProtocolType { get; set; }
        public bool? RequireClientSecret { get; set; }
        public string? ClientUri { get; set; }
        public string? LogoUri { get; set; }
        public bool? RequireConsent { get; set; }
        public bool? AllowRememberConsent { get; set; }
        public bool? RequirePkce { get; set; }
        public bool? AllowPlainTextPkce { get; set; }
        public ICollection<string>? AllowedGrantTypes { get; set; }
        public string[]? RedirectUris { get; set; }
        public string[]? PostLogoutRedirectUris { get; set; }
        public bool? RequireRequestObject { get; set; }
        public bool? AllowAccessTokensViaBrowser { get; set; }
        public string? FrontChannelLogoutUri { get; set; }
        public string? BackChannelLogoutUri { get; set; }
        public bool? FrontChannelLogoutSessionRequired { get; set; }
        public bool? BackChannelLogoutSessionRequired { get; set; }
        public bool? AllowOfflineAccess { get; set; }
        public string[]? AllowedScopes { get; set; }
        public bool? AlwaysIncludeUserClaimsInIdToken { get; set; }
        public int? IdentityTokenLifetime { get; set; }
        public int? AuthorizationCodeLifetime { get; set; }
        public string[]? AllowedIdentityTokenSigningAlgorithms { get; set; }
        public int? AbsoluteRefreshTokenLifetime { get; set; }
        public int? AccessTokenLifetime { get; set; }
        public int? SlidingRefreshTokenLifetime { get; set; }
        public int? ConsentLifetime { get; set; }
        public TokenUsage? RefreshTokenUsage { get; set; }
        public bool? UpdateAccessTokenClaimsOnRefresh { get; set; }
        public TokenExpiration? RefreshTokenExpiration { get; set; }
        public AccessTokenType? AccessTokenType { get; set; }
        public bool? EnableLocalLogin { get; set; }
        public string[]? IdentityProviderRestrictions { get; set; }
        public bool? IncludeJwtId { get; set; }
        public ClientClaim[]? Claims { get; set; }
        public bool? AlwaysSendClientClaims { get; set; }
        public string? ClientClaimsPrefix { get; set; }
        public string? PairWiseSubjectSalt { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string? UserCodeType { get; set; }
        public int? DeviceCodeLifetime { get; set; }
        public int? CibaLifetime { get; set; }
        public int? PollingInterval { get; set; }
        public string[]? AllowedCorsOrigins { get; set; }
        public IDictionary<string, string>? Properties { get; set; }
    }

    public record SecretDto(int Id, string Type, string Value, string Description, DateTime? Expiration);
    public record CreateClientSecretInputDto(int Id, string Type, string Value, string Description, DateTime? Expiration);

    public record UpdateClientSecretInputDto(int Id, string NewValue, string Description, DateTime? Expiration);

    internal record CreateUpdateApiScopeDto(
        string DisplayName,
        string Description,
        bool ShowInDiscoveryDocument,
        string[] UserClaims,
        IDictionary<string, string> Properties,
        bool Enabled,
        bool Required,
        bool Emphasize);

    internal record ApiScopeDto(
        string Name,
        string DisplayName,
        string Description,
        bool ShowInDiscoveryDocument,
        string[] UserClaims,
        IDictionary<string, string> Properties,
        bool Enabled,
        bool Required,
        bool Emphasize);

    internal record CreateUpdateApiResourceDto(
        string DisplayName,
        string[] Scopes);

    internal record ApiResourceDto(
        string Name,
        string DisplayName,
        string[] Scopes);
}
