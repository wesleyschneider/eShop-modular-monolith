using eShop.SharedKernel;

namespace eShop.Payment.Module.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent(int OrderId) : IntegrationEvent;
