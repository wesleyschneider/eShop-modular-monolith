var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddCatalogModule();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();
builder.AddDefaultOpenApi(withApiVersioning);
builder.Services.AddGrpc();

// MediatR — assemblies will be added as modules are created
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssemblyContaining<eShop.Catalog.Module.Infrastructure.CatalogContext>();
    // Module assemblies will be added here as modules are created
});

builder.Services.AddScoped<IEventBus, InProcessEventBus>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseStatusCodePages();
app.MapCatalogEndpoints();

app.Run();
