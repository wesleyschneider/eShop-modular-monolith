using eShop.SharedKernel;

namespace eShop.Payment.Module.IntegrationEvents.Events;

public record OrderPaymentSucceededIntegrationEvent(int OrderId) : IntegrationEvent;
