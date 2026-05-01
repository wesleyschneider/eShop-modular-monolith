using System.Net;
using eShop.WebApp.Components;
using eShop.ServiceDefaults;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddApplicationServices();

var app = builder.Build();

var options = new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost
};

options.KnownIPNetworks.Clear();
options.KnownProxies.Clear();

app.UseForwardedHeaders(options);

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAntiforgery();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapForwarder("/product-images/{id}", "https+http://api", "/api/catalog/items/{id}/pic");

app.MapGet("/debug-oidc", (HttpContext ctx) =>
{
    return new
    {
        Scheme = ctx.Request.Scheme,
        Host = ctx.Request.Host.Value,
        RedirectUri = $"{ctx.Request.Scheme}://{ctx.Request.Host}/signin-oidc"
    };
});

app.Run();
