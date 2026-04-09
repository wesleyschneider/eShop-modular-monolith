namespace eShop.SharedKernel.UnitTests;

[TestClass]
public class InProcessEventBusTests
{
    [TestMethod]
    public async Task PublishAsync_ShouldDelegateToMediator()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var bus = new InProcessEventBus(mediator);
        var evt = new TestIntegrationEvent();

        // Act
        await bus.PublishAsync(evt);

        // Assert
        await mediator.Received(1).Publish(
            Arg.Is<TestIntegrationEvent>(e => e.Id == evt.Id),
            Arg.Any<CancellationToken>());
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
