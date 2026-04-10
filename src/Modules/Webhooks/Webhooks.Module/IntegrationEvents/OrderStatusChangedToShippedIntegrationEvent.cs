using eShop.SharedKernel;

namespace eShop.Webhooks.Module.IntegrationEvents;

public record OrderStatusChangedToShippedIntegrationEvent(int OrderId, string OrderStatus, string BuyerName) : IntegrationEvent;
