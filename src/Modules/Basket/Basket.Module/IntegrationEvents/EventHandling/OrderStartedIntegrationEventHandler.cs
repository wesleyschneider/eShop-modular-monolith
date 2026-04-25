using eShop.Ordering.Contracts;
using eShop.Basket.Module.Repositories;
using MediatR;

namespace eShop.Basket.Module.IntegrationEvents.EventHandling;

public class OrderStartedIntegrationEventHandler(
    IBasketRepository repository,
    ILogger<OrderStartedIntegrationEventHandler> logger)
    : INotificationHandler<OrderStartedIntegrationEvent>
{
    public async Task Handle(OrderStartedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", notification.Id, notification);
        await repository.DeleteBasketAsync(notification.UserId);
    }
}
