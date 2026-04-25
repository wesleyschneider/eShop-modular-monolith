using eShop.SharedKernel;

namespace eShop.Ordering.Module.Application.IntegrationEvents;

public class OrderingIntegrationEventService(
    IEventBus eventBus,
    ILogger<OrderingIntegrationEventService> logger) : IOrderingIntegrationEventService
{
    private sealed class PendingIntegrationEvent(IntegrationEvent evt)
    {
        public IntegrationEvent Event { get; } = evt;
        public bool IsProcessing { get; set; }
    }

    private readonly List<PendingIntegrationEvent> _pendingEvents = [];

    public Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        logger.LogInformation("Enqueuing integration event {IntegrationEventId} ({@IntegrationEvent})", evt.Id, evt);
        _pendingEvents.Add(new PendingIntegrationEvent(evt));
        return Task.CompletedTask;
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        PendingIntegrationEvent pendingEvent;

        while ((pendingEvent = _pendingEvents.FirstOrDefault(e => !e.IsProcessing)) != null)
        {
            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", pendingEvent.Event.Id, pendingEvent.Event);
            pendingEvent.IsProcessing = true;
            await eventBus.PublishAsync(pendingEvent.Event);
            _pendingEvents.Remove(pendingEvent);
        }
    }
}
