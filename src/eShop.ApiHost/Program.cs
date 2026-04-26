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
