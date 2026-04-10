namespace eShop.Ordering.Module.Application.IntegrationEvents.EventHandling;

public class OrderStockConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockConfirmedIntegrationEventHandler> logger) :
    INotificationHandler<OrderStockConfirmedIntegrationEvent>
{
    public async Task Handle(OrderStockConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", notification.Id, notification);

        var command = new SetStockConfirmedOrderStatusCommand(notification.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
