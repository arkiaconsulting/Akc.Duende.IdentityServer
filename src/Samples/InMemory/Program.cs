// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using Akc.Duende.IdentityServer.Management.Api;
using Duende.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<ManagementApiOptions>(options =>
{
    options.BasePath = "/my";
});

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(SampleConstants.ApiScopes)
    .AddInMemoryApiResources(SampleConstants.ApiResources)
    .AddInMemoryClients(SampleConstants.Clients)
    .AddInMemoryManagementApi();

builder.Services.AddLocalApiAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.UseIdentityServerManagementApi()
    .RequireAuthorization(IdentityServerConstants.LocalApi.PolicyName);

app.MapRazorPages();

app.Run();
