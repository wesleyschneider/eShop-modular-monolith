using eShop.Basket.Module.Grpc;
using eShop.Basket.Module.IntegrationEvents.EventHandling;
using eShop.Basket.Module.Repositories;
using eShop.ServiceDefaults;

namespace Microsoft.Extensions.Hosting;

public static class BasketModuleExtensions
{
    public static IHostApplicationBuilder AddBasketModule(this IHostApplicationBuilder builder)
    {
        builder.AddRedisClient("redis");
        builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(BasketModuleExtensions).Assembly));

        return builder;
    }

    public static IEndpointRouteBuilder MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGrpcService<BasketService>();
        return app;
    }
}
