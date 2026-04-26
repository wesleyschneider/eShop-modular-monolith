using eShop.Webhooks.Module.Infrastructure;
using eShop.Webhooks.Module.Services;

namespace Microsoft.Extensions.Hosting;

public static class WebhooksModuleExtensions
{
    public static IHostApplicationBuilder AddWebhooksModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<WebhooksContext>("eshopdb");
        builder.Services.AddMigration<WebhooksContext>();
        builder.Services.AddHttpClient();

        builder.Services.AddTransient<IGrantUrlTesterService, GrantUrlTesterService>();
        builder.Services.AddTransient<IWebhooksRetriever, WebhooksRetriever>();
        builder.Services.AddTransient<IWebhooksSender, WebhooksSender>();

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(WebhooksModuleExtensions).Assembly));

        return builder;
    }

    public static IEndpointRouteBuilder MapWebhooksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWebHooksApi();
        return app;
    }
}
