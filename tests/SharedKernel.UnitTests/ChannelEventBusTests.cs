using System.Collections.Generic;
using System.Threading.Channels;

namespace eShop.SharedKernel.UnitTests;

[TestClass]
public class ChannelEventBusTests
{
    [TestMethod]
    public async Task PublishAsync_ShouldEnqueueEventOnChannel()
    {
        // Arrange
        var channel = Channel.CreateUnbounded<IntegrationEvent>();
        var bus = new ChannelEventBus(channel);
        var evt = new TestIntegrationEvent();

        // Act
        await bus.PublishAsync(evt);
        channel.Writer.Complete();

        // Assert
        var queued = new List<IntegrationEvent>();
        await foreach (var item in channel.Reader.ReadAllAsync())
        {
            queued.Add(item);
        }

        Assert.AreEqual(1, queued.Count);
        Assert.AreSame(evt, queued[0]);
    }

    [TestMethod]
    public void IntegrationEvent_ShouldGenerateIdAndDate()
    {
        var evt = new TestIntegrationEvent();

        Assert.AreNotEqual(Guid.Empty, evt.Id);
        Assert.IsTrue(evt.CreationDate <= DateTime.UtcNow, "CreationDate should be in the past or present");
    }

    private record TestIntegrationEvent() : IntegrationEvent;
}
