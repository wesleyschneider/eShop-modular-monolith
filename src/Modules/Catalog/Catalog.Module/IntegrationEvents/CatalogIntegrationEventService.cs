using eShop.SharedKernel;
using eShop.Catalog.Module.Infrastructure;

namespace eShop.Catalog.Module.IntegrationEvents;

public sealed class CatalogIntegrationEventService(
    ILogger<CatalogIntegrationEventService> logger,
    IEventBus eventBus,
    CatalogContext catalogContext) : ICatalogIntegrationEventService
{
    public async Task PublishThroughEventBusAsync(IntegrationEvent evt, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
        await eventBus.PublishAsync(evt, cancellationToken);
    }

    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Saving catalog changes for integration event: {IntegrationEventId}", evt.Id);
        await catalogContext.SaveChangesAsync(cancellationToken);
    }
}
