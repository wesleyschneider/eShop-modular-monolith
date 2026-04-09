using MediatR;
using eShop.Catalog.Module.IntegrationEvents.Events;

namespace eShop.Catalog.Module.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    CatalogContext catalogContext,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger) :
    INotificationHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", notification.Id, notification);

        //we're not blocking stock/inventory
        foreach (var orderStockItem in notification.OrderStockItems)
        {
            var catalogItem = await catalogContext.CatalogItems.FindAsync([orderStockItem.ProductId], cancellationToken);

            catalogItem?.RemoveStock(orderStockItem.Units);
        }

        await catalogContext.SaveChangesAsync(cancellationToken);
    }
}
