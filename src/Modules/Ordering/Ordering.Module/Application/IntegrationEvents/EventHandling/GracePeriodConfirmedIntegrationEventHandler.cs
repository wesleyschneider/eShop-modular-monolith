namespace eShop.Ordering.Module.Application.IntegrationEvents.EventHandling;

public class GracePeriodConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<GracePeriodConfirmedIntegrationEventHandler> logger) : INotificationHandler<GracePeriodConfirmedIntegrationEvent>
{
    /// <summary>
    /// Event handler which confirms that the grace period
    /// has been completed and order will not initially be cancelled.
    /// Therefore, the order process continues for validation. 
    /// </summary>
    /// <param name="event">       
    /// </param>
    /// <returns></returns>
    public async Task Handle(GracePeriodConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", notification.Id, notification);

        var command = new SetAwaitingValidationOrderStatusCommand(notification.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
