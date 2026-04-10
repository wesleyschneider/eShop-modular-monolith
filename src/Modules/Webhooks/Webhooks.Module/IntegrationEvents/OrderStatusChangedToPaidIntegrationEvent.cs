using eShop.SharedKernel;

namespace eShop.Webhooks.Module.IntegrationEvents;

public record OrderStatusChangedToPaidIntegrationEvent(int OrderId, IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;
