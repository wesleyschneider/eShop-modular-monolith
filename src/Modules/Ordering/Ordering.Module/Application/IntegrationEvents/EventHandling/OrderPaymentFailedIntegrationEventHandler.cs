namespace eShop.Ordering.Module.Application.IntegrationEvents.EventHandling;

public class OrderPaymentFailedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderPaymentFailedIntegrationEventHandler> logger) :
    INotificationHandler<OrderPaymentFailedIntegrationEvent>
{
    public async Task Handle(OrderPaymentFailedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", notification.Id, notification);

        var command = new CancelOrderCommand(notification.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
