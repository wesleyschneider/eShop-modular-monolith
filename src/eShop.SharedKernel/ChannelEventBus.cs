using System.Threading.Channels;

namespace eShop.SharedKernel;

public class ChannelEventBus(Channel<IntegrationEvent> channel) : IEventBus
{
    public Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
        => channel.Writer.WriteAsync(@event, cancellationToken).AsTask();
}
