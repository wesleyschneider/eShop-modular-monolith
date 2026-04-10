namespace eShop.Webhooks.Module.Infrastructure;

public class WebhooksContext(DbContextOptions<WebhooksContext> options) : DbContext(options)
{
    public required DbSet<WebhookSubscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("webhooks");
        modelBuilder.Entity<WebhookSubscription>(eb =>
        {
            eb.HasIndex(s => s.UserId);
            eb.HasIndex(s => s.Type);
        });
    }
}
