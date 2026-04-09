var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddGrpc();

// MediatR — assemblies will be added as modules are created
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    // Module assemblies will be added here as modules are created
});

builder.Services.AddScoped<IEventBus, InProcessEventBus>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseStatusCodePages();

app.Run();
