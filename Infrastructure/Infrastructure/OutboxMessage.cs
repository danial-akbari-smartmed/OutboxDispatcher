public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Payload { get; set; }
    public bool IsDispatched { get; set; }
}

public interface IMessageHandler
{
    Task HandleAsync(string payload);
}

public class OutboxDispatcher
{
    private readonly List<OutboxMessage> _outbox;
    private readonly IMessageHandler _handler;

    public OutboxDispatcher(List<OutboxMessage> outbox, IMessageHandler handler)
    {
        _outbox = outbox;
        _handler = handler;
    }

    public async Task DispatchPendingAsync()
    {
        var pending = _outbox.Where(m => !m.IsDispatched).ToList();

        foreach (var message in pending)
        {
            await _handler.HandleAsync(message.Payload);
            // something is missing here...
        }
    }
}
