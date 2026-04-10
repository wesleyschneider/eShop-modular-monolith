using eShop.Ordering.Module.Application.IntegrationEvents;
using eShop.Ordering.Module.Application.Queries;
using eShop.Ordering.Module.BackgroundServices;
using eShop.Ordering.Module.Infrastructure.Services;
using eShop.Ordering.Infrastructure;
using eShop.Ordering.Infrastructure.Repositories;
using eShop.Ordering.Infrastructure.Idempotency;

namespace Microsoft.Extensions.Hosting;

public static class OrderingModuleExtensions
{
    public static IHostApplicationBuilder AddOrderingModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<OrderingContext>("eshopdb");

        builder.Services.AddMigration<OrderingContext, OrderingContextSeed>();

        builder.Services.AddScoped<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddScoped<IOrderQueries, OrderQueries>();
        builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IRequestManager, RequestManager>();

        // GracePeriod background service (absorbed from OrderProcessor)
        builder.AddNpgsqlDataSource("eshopdb");
        builder.Services.AddOptions<BackgroundTaskOptions>()
            .BindConfiguration(nameof(BackgroundTaskOptions));
        builder.Services.AddHostedService<GracePeriodManagerService>();

        return builder;
    }

    public static IEndpointRouteBuilder MapOrderingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOrdersApiV1();
        return app;
    }
}
