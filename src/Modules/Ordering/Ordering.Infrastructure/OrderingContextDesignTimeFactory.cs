using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace eShop.Ordering.Infrastructure;

public class OrderingContextDesignTimeFactory : IDesignTimeDbContextFactory<OrderingContext>
{
    public OrderingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=design-time;Username=design-time;Password=design-time");

        return new OrderingContext(optionsBuilder.Options, null!);
    }
}
