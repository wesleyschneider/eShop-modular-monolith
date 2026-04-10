var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();
builder.AddCatalogModule();
builder.AddBasketModule();
builder.AddIdentityModule();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);
builder.Services.AddGrpc();

// MediatR — assemblies will be added as modules are created
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssemblyContaining<eShop.Catalog.Module.Infrastructure.CatalogContext>();
    cfg.RegisterServicesFromAssemblyContaining<eShop.Basket.Module.IntegrationEvents.EventHandling.OrderStartedIntegrationEventHandler>();
});

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

app.Run();
