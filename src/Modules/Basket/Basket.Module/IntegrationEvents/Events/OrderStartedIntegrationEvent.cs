using eShop.SharedKernel;

namespace eShop.Basket.Module.IntegrationEvents.Events;

public record OrderStartedIntegrationEvent(string UserId) : IntegrationEvent;
