using System.Net;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();
builder.AddCatalogModule();
builder.AddBasketModule();
builder.AddIdentityModule();
builder.AddOrderingModule();
builder.AddPaymentModule();
builder.AddWebhooksModule();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);
builder.Services.AddGrpc();

builder.Services.AddScoped<IEventBus, InProcessEventBus>();

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
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseIdentityModule();   // calls UseIdentityServer() — MUST be before UseAuthentication
app.UseAuthentication();
app.UseAuthorization();
app.MapCatalogEndpoints();
app.MapBasketEndpoints();
app.MapIdentityEndpoints();
app.MapOrderingEndpoints();
app.MapWebhooksEndpoints();

app.Run();
