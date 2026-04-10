namespace eShop.Webhooks.Module.Services;

public interface IWebhooksSender
{
    Task SendAll(IEnumerable<WebhookSubscription> receivers, WebhookData data);
}
