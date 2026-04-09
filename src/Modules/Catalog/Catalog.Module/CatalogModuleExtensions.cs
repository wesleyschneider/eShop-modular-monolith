using eShop.Catalog.Module;
using eShop.Catalog.Module.Infrastructure;
using eShop.Catalog.Module.IntegrationEvents;
using eShop.Catalog.Module.Services;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.Hosting;

public static class CatalogModuleExtensions
{
    public static IHostApplicationBuilder AddCatalogModule(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<CatalogContext>("eshopdb", configureDbContextOptions: dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseNpgsql(npgsqlBuilder =>
            {
                npgsqlBuilder.UseVector();
            });
        });

        builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

        builder.Services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

        builder.Services.AddOptions<CatalogOptions>()
            .BindConfiguration(nameof(CatalogOptions));

        if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        {
            builder.AddOllamaApiClient("embedding")
                .AddEmbeddingGenerator();
        }
        else if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("textEmbeddingModel")))
        {
            builder.AddOpenAIClientFromConfiguration("textEmbeddingModel")
                .AddEmbeddingGenerator();
        }

        builder.Services.AddScoped<ICatalogAI, CatalogAI>();

        return builder;
    }

    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCatalogApi();
        return app;
    }
}
