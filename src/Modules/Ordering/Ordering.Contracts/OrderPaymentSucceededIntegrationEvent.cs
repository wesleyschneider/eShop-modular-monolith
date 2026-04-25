namespace eShop.Ordering.Contracts;

public record OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public OrderPaymentSucceededIntegrationEvent(int orderId) => OrderId = orderId;
}
