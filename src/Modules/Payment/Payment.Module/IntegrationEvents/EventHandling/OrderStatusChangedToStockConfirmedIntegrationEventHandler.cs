using eShop.Payment.Module.IntegrationEvents.Events;
using eShop.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eShop.Payment.Module.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler(
    IEventBus eventBus,
    IOptions<PaymentOptions> options,
    ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger)
    : INotificationHandler<OrderStatusChangedToStockConfirmedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToStockConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", notification.Id, notification);

        IntegrationEvent orderPaymentEvent = options.Value.PaymentSucceeded
            ? new OrderPaymentSucceededIntegrationEvent(notification.OrderId)
            : new OrderPaymentFailedIntegrationEvent(notification.OrderId);

        logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", orderPaymentEvent.Id, orderPaymentEvent);

        await eventBus.PublishAsync(orderPaymentEvent);
    }
}
