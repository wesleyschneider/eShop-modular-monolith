using System.Threading.Channels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eShop.SharedKernel;

public class IntegrationEventDispatcher(
    Channel<IntegrationEvent> channel,
    IServiceScopeFactory scopeFactory,
    ILogger<IntegrationEventDispatcher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("IntegrationEventDispatcher started.");

        await foreach (var @event in channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(@event, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed dispatching integration event {EventType} ({EventId})",
                    @event.GetType().Name, @event.Id);
            }
        }

        logger.LogInformation("IntegrationEventDispatcher stopping.");
    }
}
