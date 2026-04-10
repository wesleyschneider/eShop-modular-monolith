using eShop.SharedKernel;

namespace eShop.Webhooks.Module.IntegrationEvents;

public record ProductPriceChangedIntegrationEvent(int ProductId, decimal NewPrice, decimal OldPrice) : IntegrationEvent;
