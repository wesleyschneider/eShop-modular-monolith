using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace eShop.Ordering.Infrastructure;

public class OrderingContextDesignTimeFactory : IDesignTimeDbContextFactory<OrderingContext>
{
    public OrderingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=eshopdb;Username=postgres;Password=postgres");

        return new OrderingContext(optionsBuilder.Options);
    }
}
