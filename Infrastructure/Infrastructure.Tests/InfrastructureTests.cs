namespace Infrastructure.Tests
{
    public class InfrastructureTests
    {
        [Fact]
        public async Task DispatchPendingAsync_ShouldMarkMessagesAsDispatched()
        {
            // Arrange
            var outbox = new List<OutboxMessage>
            {
                new() { Id = Guid.NewGuid(), Payload = "msg-1", IsDispatched = false },
                new() { Id = Guid.NewGuid(), Payload = "msg-2", IsDispatched = true  },
            };
            var handler = new FakeMessageHandler();
            var dispatcher = new OutboxDispatcher(outbox, handler);

            // Act
            await dispatcher.DispatchPendingAsync();

            // Assert
            Assert.True(outbox[0].IsDispatched);   // was pending — should now be true
            Assert.True(outbox[1].IsDispatched);   // was already dispatched — should still be true
            Assert.Single(handler.Handled);        // only one message should have been handled
        }

    }
    public class FakeMessageHandler : IMessageHandler
    {
        public List<string> Handled { get; } = new();
        public Task HandleAsync(string payload)
        {
            Handled.Add(payload);
            return Task.CompletedTask;
        }
    }

}