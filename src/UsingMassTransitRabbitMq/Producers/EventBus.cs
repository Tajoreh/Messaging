using MassTransit;

namespace UsingMassTransitRabbitMq.Producers;

public sealed class EventBus : IEventBus
{
    private readonly IPublishEndpoint _endpoint;

    public EventBus(IPublishEndpoint endpoint)
    {
        _endpoint = endpoint;
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    where T : class =>
      await  _endpoint.Publish<T>(message, cancellationToken);

}