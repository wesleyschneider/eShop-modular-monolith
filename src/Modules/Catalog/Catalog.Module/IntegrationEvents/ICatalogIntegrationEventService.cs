using eShop.SharedKernel;

namespace eShop.Catalog.Module.IntegrationEvents;

public interface ICatalogIntegrationEventService
{
    Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt, CancellationToken cancellationToken = default);
    Task PublishThroughEventBusAsync(IntegrationEvent evt, CancellationToken cancellationToken = default);
}
