using eShop.Catalog.Module.Infrastructure.EntityConfigurations;

namespace eShop.Catalog.Module.Infrastructure;

/// <remarks>
/// Add migrations using the following command inside the 'Catalog.Module' project directory:
///
/// dotnet ef migrations add --context CatalogContext [migration-name]
/// </remarks>
public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    public required DbSet<CatalogItem> CatalogItems { get; set; }
    public required DbSet<CatalogBrand> CatalogBrands { get; set; }
    public required DbSet<CatalogType> CatalogTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("catalog");
        builder.HasPostgresExtension("vector");
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        // Removed: builder.UseIntegrationEventLogs() — no outbox needed
    }
}
