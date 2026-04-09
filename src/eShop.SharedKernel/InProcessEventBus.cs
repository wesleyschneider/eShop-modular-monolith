using MediatR;

namespace eShop.SharedKernel;

public class InProcessEventBus(IMediator mediator) : IEventBus
{
    public Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
        => mediator.Publish(@event, cancellationToken);
}
