using MediatR;

namespace eShop.Webhooks.Module.IntegrationEvents;

public class OrderStatusChangedToShippedIntegrationEventHandler(
    IWebhooksRetriever retriever,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger) : INotificationHandler<OrderStatusChangedToShippedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToShippedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var subscriptions = await retriever.GetSubscriptionsOfType(WebhookType.OrderShipped);

        logger.LogInformation("Received OrderStatusChangedToShippedIntegrationEvent and got {SubscriptionCount} subscriptions to process", subscriptions.Count());

        var whook = new WebhookData(WebhookType.OrderShipped, notification);

        await sender.SendAll(subscriptions, whook);
    }
}
