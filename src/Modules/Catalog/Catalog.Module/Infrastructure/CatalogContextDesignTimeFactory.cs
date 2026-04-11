using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace eShop.Catalog.Module.Infrastructure;

public class CatalogContextDesignTimeFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=design-time;Username=design-time;Password=design-time",
            npgsqlOptions => npgsqlOptions.UseVector());

        return new CatalogContext(optionsBuilder.Options);
    }
}
