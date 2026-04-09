using eShop.SharedKernel;

namespace eShop.Catalog.Module.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent(int OrderId) : IntegrationEvent;
