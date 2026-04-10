using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();
builder.AddCatalogModule();
builder.AddBasketModule();
builder.AddIdentityModule();
builder.AddOrderingModule();
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
    cfg.RegisterServicesFromAssemblyContaining<eShop.Ordering.Module.Application.IntegrationEvents.OrderingIntegrationEventService>();
    // These behaviors are Ordering-specific — they only apply to IRequest<T> commands.
    // Catalog/Basket use minimal API endpoints (not MediatR commands), so these behaviors
    // will only be invoked for Ordering commands/queries.
    cfg.AddOpenBehavior(typeof(eShop.Ordering.Module.Application.Behaviors.LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(eShop.Ordering.Module.Application.Behaviors.ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(eShop.Ordering.Module.Application.Behaviors.TransactionBehavior<,>));
});
builder.Services.AddValidatorsFromAssemblyContaining<eShop.Ordering.Module.Application.IntegrationEvents.OrderingIntegrationEventService>();

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

app.Run();
