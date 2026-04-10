namespace eShop.Webhooks.Module.Services;

public interface IWebhooksRetriever
{
    Task<IEnumerable<WebhookSubscription>> GetSubscriptionsOfType(WebhookType type);
}
