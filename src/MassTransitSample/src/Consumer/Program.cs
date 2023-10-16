using Consumer;
using MassTransit;
using Producer.Event;

var bus = Bus.Factory.CreateUsingRabbitMq(a =>
{
    a.Host("rabbitmq://localhost");
    a.ReceiveEndpoint("Producer", ep =>
    {
      ep.Consumer<UserRegisteredConsumer>();
       // ep.Handler<UserRegistered>(context => Console.Out.WriteLineAsync($"Received:{context.Message.UserName}"));
    });
});
 await bus.StartAsync();
Console.ReadLine();