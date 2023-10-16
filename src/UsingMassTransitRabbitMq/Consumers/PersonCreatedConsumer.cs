using MassTransit;
using UsingMassTransitRabbitMq.Producers;

namespace UsingMassTransitRabbitMq.Consumers;

public class PersonCreatedConsumer:IConsumer<PersonCreated>
{
   public Task Consume(ConsumeContext<PersonCreated> context)
    {
        Console.WriteLine($"Person created with id:{context.Message.Id} and name:{context.Message.FullName}");

        return Task.CompletedTask;
    }
}