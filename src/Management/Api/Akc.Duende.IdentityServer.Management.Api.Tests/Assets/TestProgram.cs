// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;

namespace Akc.Duende.IdentityServer.Management.Api.Tests.Assets
{
    internal class TestProgram
    {
        public static HttpClient? HostHttpClient { get; set; }

        public const string BasePath = "/my";

        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
                Args = args
            });

            var testData = new DefaultTestData();

            builder.Services.Configure<ManagementApiOptions>(options =>
            {
                options.BasePath = BasePath;
            });

            builder.Services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(testData.Clients)
                .AddInMemoryIdentityResources(testData.IdentityResources)
                .AddInMemoryApiScopes(testData.ApiScopes)
                .AddInMemoryManagementApi();

            builder.Services.AddAuthentication()
                .AddOpenIdConnect("Testing", "Testing external login", options =>
                {
                    options.ClientId = "TestHost";
                    options.Authority = "https://localhost/fakeidp/";
                    options.CallbackPath = "/signin-testing";
                    options.Backchannel = HostHttpClient!;
                    options.ProtocolValidator.RequireNonce = false;
                    options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateLifetime = false;
                    options.TokenValidationParameters.RequireSignedTokens = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.SignatureValidator = (string token, TokenValidationParameters p) =>
                    {
                        var securityToken = new JwtSecurityToken(token);
                        securityToken.Payload.Remove("nonce");

                        return securityToken;
                    };
                });

            builder.Services.AddLocalApiAuthentication();

            builder.Services.AddIdentityServerLocalJwtBearerByPass(); // for tests purpose

            builder.Services.AddSingleton(testData);

            var app = builder.Build();

            app.UseIdentityServer();
            app.UseAuthorization();

            app
            .UseIdentityServerManagementApi()
            .RequireAuthorization(IdentityServerConstants.LocalApi.PolicyName);

            app.MapGet("/home/error", async (string errorId, IIdentityServerInteractionService interaction) =>
            {
                var error = await interaction.GetErrorContextAsync(errorId);

                return Results.Ok(error);
            });

            app.MapGet("/account/login", async (string returnUrl, HttpContext ctx) =>
            {
                var props = new AuthenticationProperties
                {
                    RedirectUri = "/login/callback",
                    Items =
                    {
                        { "scheme", "AAD" },
                        { "returnUrl", returnUrl }
                    }
                };

                return Results.Challenge(props, new List<string> { "Testing" });


                //var user = new IdentityServerUser(Guid.NewGuid().ToString())
                //{
                //    DisplayName = "toto",
                //    IdentityProvider = "test"
                //};

                //await ctx.SignInAsync(user);

                //return Results.ch.Redirect(returnUrl);

                //var returnUri = new Uri(new Uri("https://localhost/"), returnUrl);
                //var query = returnUri.Query;
                //var redirectUri = HttpUtility.ParseQueryString(query)["redirect_uri"];

                //var callbackUri = new UriBuilder(redirectUri!) { Query = "?code=anycode" };
                //return Results.Redirect(callbackUri.ToString());
            });

            app.MapGet("/fakeidp/.well-known/openid-configuration", () =>
            {
                var fg = new IdentityModel.Client.DiscoveryDocumentResponse();

                return Results.Json(new
                {
                    authorization_endpoint = "https://localhost/fakeidp/authorize"
                });
            });

            app.MapGet("/fakeidp/authorize", async (string redirect_uri, string state) =>
            {
                var uri = new UriBuilder(redirect_uri)
                {
                    Query = $"?state={state}"
                };

                var message = new Dictionary<string, string> {
                    { "id_token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6ImwtSjZ3X1BJLUhHTVE2YUJubFMxVXBSbklKTDFyMkZUV2pmSzMtWmxWbXcifQ.eyJleHAiOjE2NDkwMjExMjMsIm5iZiI6MTY0OTAxNzUyMywidmVyIjoiMS4wIiwiaXNzIjoiaHR0cHM6Ly9pbnRlLXNpZ25pbi5jZWdpZC5jb20vMGRhZjkxNjAtZGMwMy00YWRiLThkYTktZWVmZmRhMDBiYTQ2L3YyLjAvIiwic3ViIjoiRTQ0QUE2MjNGQUQ0NURBN0IzNDEiLCJhdWQiOiI4MGI2NmVhZS01YjViLTRiY2ItYjQyMS00NTVjMzViZmE1ZWQiLCJhY3IiOiJiMmNfMWFfcnBfY3AtY2RlIiwibm9uY2UiOiI2Mzc4NDYxNDI3MTkxNDc0MDQuT1dWall6QmlabU10WlRVMk5TMDBNbU5qTFRsaFltRXRORFV3TjJKa00ySTJOREk1TkRSak1tTTFORGt0WkRFM05DMDBORFE1TFdJMlpESXRNak0wTlRKa1kyRTFaRGxoIiwiaWF0IjoxNjQ5MDE3NTIzLCJhdXRoX3RpbWUiOjE2NDkwMTc1MjMsImlkcCI6IiIsInNpZ25pbl9jZWdpZF9zbG90IjoiQmx1ZSIsImVtYWlsIjoibGdlcmFyZEBjZWdpZC5jb20iLCJnaXZlbl9uYW1lIjoiTG91aXMiLCJmYW1pbHlfbmFtZSI6IkdFUkFSRCIsInBob25lX251bWJlciI6IjA0MjYyOTUwMDAiLCJtb2JpbGVfcGhvbmUiOiIwNzYwMTg3MDE0Iiwic2ljX2lkIjoiOTk5ODAwMDExMDc2IiwiYWNjb3VudF9pZCI6Ijk5OTgwMDAxIiwiY29kZWRfdGl0bGUiOiJNIiwiY29kZWRfbGFuZ3VhZ2UiOiJmci1GUiIsImNvZGVkX2pvYl90aXRsZSI6IkRWUCIsImFjY291bnRfbmFtZSI6IkNFR0lEIEdST1VQIERJUkVDVElPTlMgVFJBTlNWRVJTRVMiLCJhY2NvdW50aW5nX3N0YXR1cyI6IiIsInJvbGUiOlsiSVRTTV9TVEQiLCJBX1RFTEVDSEFSR0VNRU5UIiwiQV9DTE9VRF9DT05UUk9MIiwiQV9ZRVMiLCJBX0ZBUSIsIkFfUFJFRkVSRU5DRV9GMTEwIiwiQV9QUkVGRVJFTkNFX0YxMjAiLCJBX1BSRUZFUkVOQ0VfRjEyNSIsIkFfUFJFRkVSRU5DRV9GMTMwIiwiQV9QUkVGRVJFTkNFX0YxNTAiLCJBX1BSRUZFUkVOQ0VfRjE1MiIsIkFfUFJFRkVSRU5DRV9GMTU1IiwiQV9QUkVGRVJFTkNFX0YxNTciLCJBX1BSRUZFUkVOQ0VfRjE2MCIsIkFfUFJFRkVSRU5DRV9GMTcwIiwiQV9QUkVGRVJFTkNFX0YxNzMiLCJBX1BSRUZFUkVOQ0VfRjE4MCIsIkFfUFJFRkVSRU5DRV9GMTkwIiwiQV9QUkVGRVJFTkNFX0YyMTAiLCJBX1BSRUZFUkVOQ0VfRjIzMCIsIkFfUFJFRkVSRU5DRV9GMjM1IiwiQV9QUkVGRVJFTkNFX0YyMzgiLCJBX1BSRUZFUkVOQ0VfRjI0MCIsIkFfUFJFRkVSRU5DRV9GMjUwIiwiQV9QUkVGRVJFTkNFX0YyNjAiLCJBX1BSRUZFUkVOQ0VfRjI2NSIsIkFfUFJFRkVSRU5DRV9GMjcwIiwiQV9QUkVGRVJFTkNFX0YyODAiLCJBX1BSRUZFUkVOQ0VfRjI5NSIsIkFfUFJFRkVSRU5DRV9GMzQwIiwiQV9QUkVGRVJFTkNFX0Y5MDAiLCJBX0NFR0lETElGRV9DT00iLCJBX0NFR0lEX0NERSIsIlNBQVNfTkVYT0RfMDEiLCJBX3Rlc3Q0MDgwMjE2IiwiQV90ZXN0NTA4MDIxNiIsIkFfU05fR1JPVVBFIiwiQURNLTEiLCJBX1BBUkNfU0VSSUEiLCJBX0VTVUJTQ1JJQkUiLCJDTF9Db25uZWN0X1V0aWxpc2F0ZXVyIiwiU0FBU19FR0FMSVRFX0ZIIl0sImV4dGVuZGVkX3JvbGUiOlsiSVRTTV9TVEQsIDk5OTgwMDAxIiwiQV9URUxFQ0hBUkdFTUVOVCwgOTk5ODAwMDEiLCJBX0NMT1VEX0NPTlRST0wsIDk5OTgwMDAxIiwiQV9ZRVMsIDk5OTgwMDAxIiwiQV9GQVEsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxMTAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxMjAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxMjUsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxMzAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxNTAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxNTIsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxNTUsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxNTcsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxNjAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxNzAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxNzMsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxODAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YxOTAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyMTAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyMzAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyMzUsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyMzgsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyNDAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyNTAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyNjAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyNjUsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyNzAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyODAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YyOTUsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0YzNDAsIDk5OTgwMDAxIiwiQV9QUkVGRVJFTkNFX0Y5MDAsIDk5OTgwMDAxIiwiQV9DRUdJRExJRkVfQ09NLCA5OTk4MDAwMSIsIkFfQ0VHSURfQ0RFLCA5OTk4MDAwMSIsIlNBQVNfTkVYT0RfMDEsIDk5OTgwMDAxIiwiQV90ZXN0NDA4MDIxNiwgOTk5ODAwMDEiLCJBX3Rlc3Q1MDgwMjE2LCA5OTk4MDAwMSIsIkFfU05fR1JPVVBFLCA5OTk4MDAwMSIsIkFETS0xLCA5OTk4MDAwMSIsIkFfUEFSQ19TRVJJQSwgOTk5ODAwMDEiLCJBX0VTVUJTQ1JJQkUsIDk5OTgwMDAxIiwiQURNLTEsIDkwMDAwMDIyIiwiQ0xfQ29ubmVjdF9VdGlsaXNhdGV1ciwgOTk5ODAwMDEiLCJBRE0tMSwgOTAwMjI4NDMiLCJTQUFTX0VHQUxJVEVfRkgsIDkwMDIyODQzIl19.rMlA-UKxZ-w07Uij07Hggpb8y6bJAkanJgPYt36FU9PJCQIrz-ZttshJgTutxajO_PF7w0Y-RWXiF3krlsjx6hlnhJxhtxesCBpO6ymfzGzfC4eTrqktxNvhooKCvCYNDU38t3fV0WrXpOjoabd-pAv-0rcqIk55fUIgCv1YjXRbLx86Ns3_maqooDJp_yIkwzTcyVl-t1o7u59HoVx8Y6Frtz_G4cEDvqucZcJENzqF60p-BzUIdXS3Z3AZ_WqubH0QhZJKYUPy3IL9hAkPHdN84rge9nsoptAlV4aFSG31XH2OHb5O5BGZDhrajv5k0vn6ZpKH_lEeooq9MYIgvQ"},
                    { "state", state},
                };

                using var response = await HostHttpClient!.PostAsync(uri.ToString(),
                    new FormUrlEncodedContent(message));

                response.EnsureSuccessStatusCode();
            });

            app.Run();
        }
    }
}