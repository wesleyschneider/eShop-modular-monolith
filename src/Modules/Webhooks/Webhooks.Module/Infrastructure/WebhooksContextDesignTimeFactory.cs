using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace eShop.Webhooks.Module.Infrastructure;

public class WebhooksContextDesignTimeFactory : IDesignTimeDbContextFactory<WebhooksContext>
{
    public WebhooksContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebhooksContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=design-time;Username=design-time;Password=design-time");

        return new WebhooksContext(optionsBuilder.Options);
    }
}
