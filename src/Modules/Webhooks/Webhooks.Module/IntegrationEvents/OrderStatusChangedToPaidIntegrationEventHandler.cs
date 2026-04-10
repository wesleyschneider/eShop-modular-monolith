using MediatR;

namespace eShop.Webhooks.Module.IntegrationEvents;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    IWebhooksRetriever retriever,
    IWebhooksSender sender,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger) : INotificationHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var subscriptions = await retriever.GetSubscriptionsOfType(WebhookType.OrderPaid);

        logger.LogInformation("Received OrderStatusChangedToPaidIntegrationEvent and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());

        var whook = new WebhookData(WebhookType.OrderPaid, notification);

        await sender.SendAll(subscriptions, whook);
    }
}
