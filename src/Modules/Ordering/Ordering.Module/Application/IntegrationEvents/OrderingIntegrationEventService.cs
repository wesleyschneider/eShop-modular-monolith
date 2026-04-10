using eShop.SharedKernel;

namespace eShop.Ordering.Module.Application.IntegrationEvents;

public class OrderingIntegrationEventService(
    IEventBus eventBus,
    ILogger<OrderingIntegrationEventService> logger) : IOrderingIntegrationEventService
{
    private readonly List<IntegrationEvent> _pendingEvents = [];

    public Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        logger.LogInformation("Enqueuing integration event {IntegrationEventId} ({@IntegrationEvent})", evt.Id, evt);
        _pendingEvents.Add(evt);
        return Task.CompletedTask;
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        foreach (var evt in _pendingEvents.ToList())
        {
            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
            await eventBus.PublishAsync(evt);
        }
        _pendingEvents.Clear();
    }
}
