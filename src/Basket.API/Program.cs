var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();

var app = builder.Build();

app.UseServiceDefaults();

app.MapDefaultEndpoints();

app.MapGrpcService<BasketService>();

app.Run();
