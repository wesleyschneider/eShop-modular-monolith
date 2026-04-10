using eShop.SharedKernel;

namespace eShop.Payment.Module.IntegrationEvents.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent(int OrderId) : IntegrationEvent;
