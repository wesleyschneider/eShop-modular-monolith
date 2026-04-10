namespace eShop.Ordering.Module.Application.IntegrationEvents.EventHandling;
public class OrderStockRejectedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockRejectedIntegrationEventHandler> logger) : INotificationHandler<OrderStockRejectedIntegrationEvent>
{
    public async Task Handle(OrderStockRejectedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", notification.Id, notification);

        var orderStockRejectedItems = notification.OrderStockItems
            .FindAll(c => !c.HasStock)
            .Select(c => c.ProductId)
            .ToList();

        var command = new SetStockRejectedOrderStatusCommand(notification.OrderId, orderStockRejectedItems);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
