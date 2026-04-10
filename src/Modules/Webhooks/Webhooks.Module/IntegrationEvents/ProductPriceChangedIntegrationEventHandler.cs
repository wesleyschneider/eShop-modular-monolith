using MediatR;

namespace eShop.Webhooks.Module.IntegrationEvents;

public class ProductPriceChangedIntegrationEventHandler : INotificationHandler<ProductPriceChangedIntegrationEvent>
{
    public Task Handle(ProductPriceChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
