using eShop.Ordering.Module.Application.Behaviors;
using eShop.Ordering.Module.Application.IntegrationEvents;
using eShop.Ordering.Module.Application.Queries;
using FluentValidation;
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
        // Pooling is disabled because of the following error:
        // Unhandled exception. System.InvalidOperationException:
        // The DbContext of type 'OrderingContext' cannot be pooled because it does not have a public constructor accepting a single parameter of type DbContextOptions or has more than one constructor.
        builder.Services.AddDbContext<OrderingContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("eshopdb"));
        });
        builder.EnrichNpgsqlDbContext<OrderingContext>();

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

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(OrderingModuleExtensions).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
        builder.Services.AddValidatorsFromAssembly(typeof(OrderingModuleExtensions).Assembly);

        return builder;
    }

    public static IEndpointRouteBuilder MapOrderingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOrdersApiV1();
        return app;
    }
}
